using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BuyMyHouse.Models;

namespace BuyMyHouse.Functions
{
    public class SubmitApplicationFunction
    {
        private readonly ILogger<SubmitApplicationFunction> _logger;

        public SubmitApplicationFunction(ILogger<SubmitApplicationFunction> logger)
        {
            _logger = logger;
        }

        [Function("SubmitApplicationFunction")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "submit-application")] HttpRequest req)
        {
            _logger.LogInformation("Processing mortgage application submission.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<MortgageApplication>(requestBody);

            // Validate input
            if (data == null || string.IsNullOrEmpty(data.CustomerID) || 
                data.HouseID <= 0 || data.Income <= 0 || data.CreditScore <= 0 ||
                string.IsNullOrEmpty(data.CustomerEmail) || !IsValidEmail(data.CustomerEmail))
            {
                return new BadRequestObjectResult("Invalid application data.");
            }

            try
            {
                // Create a reference to the Table
                string? connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("AzureWebJobsStorage environment variable is not set.");
                }
                var tableClient = new TableClient(connectionString, "Applications");

                // Create the table if it doesn't exist
                await tableClient.CreateIfNotExistsAsync();

                // Create a new application entity
                var applicationEntity = new TableEntity(data.CustomerID, Guid.NewGuid().ToString())
                {
                    { "HouseID", data.HouseID },
                    { "ApplicationDate", DateTime.UtcNow },
                    { "Status", "submitted" },
                    { "Income", data.Income },
                    { "CreditScore", data.CreditScore },
                    { "CustomerEmail", data.CustomerEmail },
                    { "MortgageOfferID", null }, // Initially null
                    { "OfferUrl", null } // Initially null
                };

                // Insert the entity into the table
                await tableClient.AddEntityAsync(applicationEntity);

                _logger.LogInformation("Application successfully saved to Cosmos Table API.");

                return new OkObjectResult("Application submitted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while processing the application: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Helper method to validate email
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

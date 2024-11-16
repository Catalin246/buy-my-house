using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace BuyMyHouse.Functions
{
    public class BatchProcessFunction
    {
        private readonly ILogger<BatchProcessFunction> _logger;

        public BatchProcessFunction(ILogger<BatchProcessFunction> logger)
        {
            _logger = logger;
        }

        [Function("BatchProcessFunction")]
        public async Task Run(
        //[TimerTrigger("0 0 0 * * *")] TimerInfo timer) // Executes daily at midnight
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test-batch-process")] HttpRequestData req)
        {
            _logger.LogInformation("Batch processing started at: {Time}", DateTime.UtcNow);

            try
            {
                // Connect to Azure Table Storage
                string? connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("AzureWebJobsStorage environment variable is not set.");
                }
                var tableClient = new TableClient(connectionString, "Applications");

                // Query all applications with status "submitted"
                var applications = tableClient.Query<ApplicationEntity>(app => app.Status == "submitted");

                foreach (var application in applications)
                {
                    // Generate a mortgage offer
                    var offer = GenerateMortgageOffer(application);

                    // Save the offer to Blob Storage
                    string blobUrl = await SaveOfferToBlobAsync(connectionString, offer, application.RowKey);

                    // Update application status in Table Storage
                    application.Status = "offer-generated";
                    application.MortgageOfferID = offer.OfferID;
                    application.OfferUrl = blobUrl; // Save the offer URL
                    await tableClient.UpdateEntityAsync(application, ETag.All);

                    _logger.LogInformation("Processed application for CustomerID: {CustomerID}", application.PartitionKey);
                }

                _logger.LogInformation("Batch processing completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred during batch processing: {Message}", ex.Message);
            }
        }

        private MortgageOffer GenerateMortgageOffer(ApplicationEntity application)
        {
            // Simple mortgage offer generation logic
            decimal loanAmount = application.Income * 4; // Loan amount is 4x income
            double interestRate = application.CreditScore >= 700 ? 3.5 : 5.0; // Lower rate for high credit score

            return new MortgageOffer
            {
                OfferID = Guid.NewGuid().ToString(),
                CustomerID = application.PartitionKey,
                LoanAmount = loanAmount,
                InterestRate = interestRate,
                Terms = "30 years fixed",
                OfferDate = DateTime.UtcNow
            };
        }

        private async Task<string> SaveOfferToBlobAsync(string connectionString, MortgageOffer offer, string applicationID)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("mortgage-offers");

            // Create the Blob container if it doesn't exist
            await containerClient.CreateIfNotExistsAsync();

            // Save the mortgage offer as a JSON file in the Blob container
            var blobClient = containerClient.GetBlobClient($"{applicationID}.json");
            var offerJson = JsonSerializer.Serialize(offer);

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(offerJson)))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString(); // Return the URL of the Blob
        }
    }

    public class MortgageOffer
    {
        public required string OfferID { get; set; }
        public required string CustomerID { get; set; }
        public decimal LoanAmount { get; set; }
        public double InterestRate { get; set; }
        public required string Terms { get; set; }
        public DateTime OfferDate { get; set; }
    }

    public class ApplicationEntity : ITableEntity
    {
        public required string PartitionKey { get; set; } // CustomerID
        public required string RowKey { get; set; } // ApplicationID
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public int HouseID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public required string Status { get; set; }
        public decimal Income { get; set; }
        public int CreditScore { get; set; }
        public string? MortgageOfferID { get; set; }
        public string? OfferUrl { get; set; } // New property for the Blob URL
    }
}

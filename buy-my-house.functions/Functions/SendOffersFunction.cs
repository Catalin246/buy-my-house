using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using BuyMyHouse.Models;

namespace BuyMyHouse.Functions
{
    public class SendOffersFunction
    {
        private readonly ILogger<SendOffersFunction> _logger;

        public SendOffersFunction(ILogger<SendOffersFunction> logger)
        {
            _logger = logger;
        }

        [Function("SendOffersFunction")]
        public async Task Run(
        [TimerTrigger("0 0 5 * * *")] TimerInfo timer) // Executes at 5 AM daily
        //[HttpTrigger(AuthorizationLevel.Function, "get", Route = "test-send-offers")] HttpRequestData req) // For testing purpose
        {
            _logger.LogInformation("SendOffersFunction triggered at: {Time}", DateTime.UtcNow);

            try
            {
                // Connect to Azure Table Storage
                string? connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("AzureWebJobsStorage environment variable is not set.");
                }

                var applicationsTable = new TableClient(connectionString, "Applications");
                var offersTable = new TableClient(connectionString, "Offers");

                // Ensure the Offers table exists
                await offersTable.CreateIfNotExistsAsync();
                _logger.LogInformation("Offers table verified or created.");

                // Query applications with status "offer-generated"
                var applications = applicationsTable.Query<OfferEntity>(app => app.Status == "offer-generated");

                foreach (var application in applications)
                {
                    // Prepare and send email
                    var emailSent = await SendEmailAsync(application.CustomerEmail, application.OfferUrl);
                    if (emailSent)
                    {
                        // Save offer to the Offers table
                        var offer = new OfferEntity
                        {
                            PartitionKey = application.PartitionKey, // Use CustomerID/Email as PartitionKey
                            RowKey = Guid.NewGuid().ToString(), // Unique Offer ID
                            CustomerEmail = application.CustomerEmail,
                            OfferUrl = application.OfferUrl,
                            Status = "offer-sent"
                        };

                        await offersTable.AddEntityAsync(offer);

                        // Update application status in Applications table
                        application.Status = "offer-sent";
                        await applicationsTable.UpdateEntityAsync(application, ETag.All);

                        _logger.LogInformation("Offer sent and recorded for CustomerID: {CustomerID}", application.PartitionKey);
                    }
                }

                _logger.LogInformation("SendOffersFunction completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while sending offers: {Message}", ex.Message);
            }
        }

        private async Task<bool> SendEmailAsync(string toEmail, string offerUrl)
        {
            try
            {
                // Fetch and validate environment variables
                string smtpHost = Environment.GetEnvironmentVariable("SmtpHost") 
                                ?? throw new InvalidOperationException("Environment variable 'SmtpHost' is not set.");

                string smtpUser = Environment.GetEnvironmentVariable("SmtpUser") 
                                ?? throw new InvalidOperationException("Environment variable 'SmtpUser' is not set.");

                string smtpPass = Environment.GetEnvironmentVariable("SmtpPassword") 
                                ?? throw new InvalidOperationException("Environment variable 'SmtpPassword' is not set.");

                string smtpPortValue = Environment.GetEnvironmentVariable("SmtpPort") 
                                    ?? throw new InvalidOperationException("Environment variable 'SmtpPort' is not set.");

                if (!int.TryParse(smtpPortValue, out int smtpPort))
                {
                    throw new InvalidOperationException($"Environment variable 'SmtpPort' is not a valid integer: {smtpPortValue}");
                }

                string fromEmail = Environment.GetEnvironmentVariable("FromEmail") 
                                ?? throw new InvalidOperationException("Environment variable 'FromEmail' is not set.");

                // Ensure email address and offer URL are not null
                if (string.IsNullOrEmpty(toEmail))
                {
                    throw new ArgumentException("Recipient email address (toEmail) cannot be null or empty.", nameof(toEmail));
                }

                if (string.IsNullOrEmpty(offerUrl))
                {
                    throw new ArgumentException("Offer URL (offerUrl) cannot be null or empty.", nameof(offerUrl));
                }

                // Email content
                var subject = "Your Mortgage Offer is Ready!";
                var body = $"<p>Your mortgage offer is ready. Please click the link below to view it:</p>" +
                        $"<a href=\"{offerUrl}\">View Mortgage Offer</a><br><br>" +
                        $"<p>This offer is valid for 24 hours.</p>";

                // Configure SMTP client
                var smtpClient = new SmtpClient(smtpHost)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = false // set true in production
                };

                // Create and send email
                var mailMessage = new MailMessage(fromEmail, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation("Email sent to: {Email}", toEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email to {Email}: {Message}", toEmail, ex.Message);
                return false;
            }
        }
    }
}

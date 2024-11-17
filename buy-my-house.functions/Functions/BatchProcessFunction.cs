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
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using BuyMyHouse.Models;

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
        [TimerTrigger("0 0 0 * * *")] TimerInfo timer) // Executes daily at midnight
        //[HttpTrigger(AuthorizationLevel.Function, "get", Route = "test-batch-process")] HttpRequestData req) // For testing purpose
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

                    // Save the offer PDF to Blob Storage
                    string blobUrl = await SaveOfferPdfToBlobAsync(connectionString, offer, application.RowKey);

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

        private async Task<string> SaveOfferPdfToBlobAsync(string connectionString, MortgageOffer offer, string applicationID)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("mortgage-offers");

            // Create the Blob container if it doesn't exist
            await containerClient.CreateIfNotExistsAsync();

            // Generate the mortgage offer PDF
            string pdfFilePath = GeneratePdfOffer(offer, applicationID);

            // Save the PDF to Blob Storage
            var blobClient = containerClient.GetBlobClient($"{applicationID}.pdf");
            using (var stream = File.OpenRead(pdfFilePath))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            // Delete the local PDF file after upload
            File.Delete(pdfFilePath);

            return blobClient.Uri.ToString(); // Return the URL of the Blob
        }

        private string GeneratePdfOffer(MortgageOffer offer, string applicationID)
        {
            var pdfFilePath = Path.Combine(Path.GetTempPath(), $"{applicationID}_Offer.pdf");

            try
            {
                // Create a new PDF document
                using var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Verdana", 20, XFontStyle.Bold);
                var fontContent = new XFont("Verdana", 12, XFontStyle.Regular);

                // Draw the title
                gfx.DrawString("Mortgage Offer", fontTitle, XBrushes.Black,
                    new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);

                // Draw the content
                gfx.DrawString($"Customer ID: {offer.CustomerID}", fontContent, XBrushes.Black,
                    new XRect(20, 80, page.Width - 40, page.Height - 80), XStringFormats.TopLeft);

                gfx.DrawString($"Loan Amount: ${offer.LoanAmount:N2}", fontContent, XBrushes.Black,
                    new XRect(20, 100, page.Width - 40, page.Height - 100), XStringFormats.TopLeft);

                gfx.DrawString($"Interest Rate: {offer.InterestRate}%", fontContent, XBrushes.Black,
                    new XRect(20, 120, page.Width - 40, page.Height - 120), XStringFormats.TopLeft);

                gfx.DrawString($"Terms: {offer.Terms}", fontContent, XBrushes.Black,
                    new XRect(20, 140, page.Width - 40, page.Height - 140), XStringFormats.TopLeft);

                gfx.DrawString($"Offer Date: {offer.OfferDate:yyyy-MM-dd}", fontContent, XBrushes.Black,
                    new XRect(20, 160, page.Width - 40, page.Height - 160), XStringFormats.TopLeft);

                // Save the document
                document.Save(pdfFilePath);
                _logger.LogInformation("PDF generated at: {PdfPath}", pdfFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to generate PDF for ApplicationID: {ApplicationID}. Error: {Message}", applicationID, ex.Message);
                throw;
            }

            return pdfFilePath;
        }
    }
}

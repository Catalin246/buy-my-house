using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BuyMyHouse.Functions
{
    public class EmailDispatchFunction
    {
        private readonly ILogger<EmailDispatchFunction> _logger;

        public EmailDispatchFunction(ILogger<EmailDispatchFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(EmailDispatchFunction))]
        public void Run([QueueTrigger("email-queue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}

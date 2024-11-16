using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BuyMyHouse.Functions
{
    public class BatchFunction
    {
        private readonly ILogger _logger;

        public BatchFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BatchFunction>();
        }

        [Function("BatchFunction")]
        public void Run([TimerTrigger("0 0 1 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}

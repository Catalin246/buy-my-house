using Azure;
using Azure.Data.Tables;

namespace BuyMyHouse.Models
{
    public class OfferEntity : ITableEntity
    {
        public required string PartitionKey { get; set; }
        public required string RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public required string CustomerEmail { get; set; }
        public required string OfferUrl { get; set; }
        public required string Status { get; set; }
    }
}
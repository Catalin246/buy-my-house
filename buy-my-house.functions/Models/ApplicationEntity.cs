using Azure;
using Azure.Data.Tables;

namespace BuyMyHouse.Models
{
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
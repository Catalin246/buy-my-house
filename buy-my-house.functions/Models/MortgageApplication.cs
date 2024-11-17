namespace BuyMyHouse.Models
{ 
    public class MortgageApplication
    {
        public required string CustomerID { get; set; }
        public int HouseID { get; set; }
        public decimal Income { get; set; }
        public int CreditScore { get; set; }
        public required string CustomerEmail { get; set; }
    }
}
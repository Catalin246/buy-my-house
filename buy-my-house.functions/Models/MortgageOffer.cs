namespace BuyMyHouse.Models
{
    public class MortgageOffer
    {
        public required string OfferID { get; set; }
        public required string CustomerID { get; set; }
        public decimal LoanAmount { get; set; }
        public double InterestRate { get; set; }
        public required string Terms { get; set; }
        public DateTime OfferDate { get; set; }
    }
}
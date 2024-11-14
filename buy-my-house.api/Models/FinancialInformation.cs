using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyMyHouse.Models
{
    public class FinancialInformation
    {
        [Key]
        public int FinancialInformationID { get; set; } // Primary Key

        [Required]
        public decimal Income { get; set; } // Customer’s income

        [Required]
        public int CreditScore { get; set; } // Customer’s credit score
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BuyMyHouse.Models
{
    public class FinancialInformation
    {
        [Key]
        public int FinancialInformationID { get; set; } // Primary Key

        [Precision(18, 2)]
        public decimal? Income { get; set; } // Customer’s income

        public int? CreditScore { get; set; } // Customer’s credit score
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyMyHouse.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public required string Name { get; set; } // Customer’s full name

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; } // Customer’s email address

        [Phone]
        [StringLength(15)]
        public string? PhoneNumber { get; set; } // Customer’s phone number

        public int FinancialInformationID { get; set; } // Foreign Key to FinancialInformation

        [ForeignKey("FinancialInformationID")]
        public required FinancialInformation FinancialInformation { get; set; } // Navigation property
    }
}

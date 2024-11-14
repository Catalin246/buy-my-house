using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BuyMyHouse.Models
{
    public class House
    {
        [Key]
        public int HouseID { get; set; } // Primary Key

        [Required]
        [StringLength(255)]
        public string? Location { get; set; } // Address or location of the property

        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; } // Listing price of the house

        [Required]
        public int Bedrooms { get; set; } // Number of bedrooms

        [Required]
        public int Bathrooms { get; set; } // Number of bathrooms

        public bool Garden { get; set; } // Indicates if there is a garden

        public bool Balcony { get; set; } // Indicates if there is a balcony

        [StringLength(10)]
        public string? EnergyClass { get; set; } // Energy efficiency rating of the house
    }
}

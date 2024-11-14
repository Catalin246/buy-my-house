using BuyMyHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace BuyMyHouse.DAL
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public required DbSet<House> Houses { get; set; }
        public required DbSet<Customer> Customers { get; set; }
        public required DbSet<FinancialInformation> FinancialInformations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure any relationships, constraints, etc. if necessary
            base.OnModelCreating(modelBuilder);
        }
    }
}

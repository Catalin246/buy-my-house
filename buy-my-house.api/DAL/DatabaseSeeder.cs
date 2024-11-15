using BuyMyHouse.Models;
using BuyMyHouse.Fakers;
using System.Linq;

namespace BuyMyHouse.DAL
{
    public class DatabaseSeeder
    {
        private readonly DatabaseContext _context;

        public DatabaseSeeder(DatabaseContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            // Check if the database is already seeded with houses 
            if (!_context.Houses.Any())
            {
                // Use FakeHouseGenerator to create 5 fake houses
                var fakeHouses = FakeHouseGenerator.GenerateHouses(5);

                // Add data to the database
                _context.Houses.AddRange(fakeHouses);

                // Save changes to the database
                _context.SaveChanges();
            }

            // Check if the database is already seeded with customers
            if (!_context.Customers.Any())
            {
                 // Use FakeCustomerGenerator to create 5 fake houses
                var fakeCustomers = FakeCustomerGenerator.GenerateCustomers(5);

                // Add data to the database
                _context.Customers.AddRange(fakeCustomers);

                // Save changes to the database
                _context.SaveChanges();
            }
        }
    }
}

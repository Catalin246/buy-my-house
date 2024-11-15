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
            // Check if the database is already seeded
            if (!_context.Houses.Any())
            {
                // Use FakeHouseGenerator to create 10 fake houses
                var fakeHouses = FakeHouseGenerator.GenerateHouses(10);

                // Add data to the database
                _context.Houses.AddRange(fakeHouses);

                // Save changes to the database
                _context.SaveChanges();
            }
        }
    }
}

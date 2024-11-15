using Bogus;
using BuyMyHouse.Models;
using System.Collections.Generic;

namespace BuyMyHouse.Fakers
{
    public static class FakeHouseGenerator
    {
        public static List<House> GenerateHouses(int count)
        {
            var faker = new Faker<House>()
                .RuleFor(h => h.Location, f => f.Address.FullAddress()) // Fake address
                .RuleFor(h => h.Price, f => f.Random.Decimal(100000, 1000000)) // Price between $100,000 and $1,000,000
                .RuleFor(h => h.Bedrooms, f => f.Random.Int(1, 5)) // 1 to 5 bedrooms
                .RuleFor(h => h.Bathrooms, f => f.Random.Int(1, 3)) // 1 to 3 bathrooms
                .RuleFor(h => h.Garden, f => f.Random.Bool()) // Random true/false
                .RuleFor(h => h.Balcony, f => f.Random.Bool()) // Random true/false
                .RuleFor(h => h.EnergyClass, f => f.Random.String2(1, "AABBBCCCDDD")); // Random energy class

            return faker.Generate(count); // Generate the specified number of houses
        }
    }                                                                       
}

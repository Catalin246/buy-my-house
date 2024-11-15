using Bogus;
using BuyMyHouse.Models;
using System.Collections.Generic;

namespace BuyMyHouse.Fakers
{
    public static class FakeCustomerGenerator
    {
        public static List<Customer> GenerateCustomers(int count)
        {
            var faker = new Faker<Customer>()
                .RuleFor(c => c.Name, f => f.Name.FullName()) // Random full name
                .RuleFor(c => c.Email, f => f.Internet.Email()) // Random email
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("(###) ###-####")); // Random phone number

            return faker.Generate(count);
        }
    }
}

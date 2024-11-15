using Bogus;
using BuyMyHouse.Models;
using System.Collections.Generic;

namespace BuyMyHouse.Fakers
{
    public static class FakeFinancialInformationGenerator
    {
        public static List<FinancialInformation> GenerateFinancialInformation(int count)
        {
            var faker = new Faker<FinancialInformation>()
                .RuleFor(fi => fi.Income, f => f.Finance.Amount(30000, 200000)) // Random income
                .RuleFor(fi => fi.CreditScore, f => f.Random.Int(300, 850)); // Random credit score

            return faker.Generate(count); // Generate the specified number of financial information entries
        }
    }
}

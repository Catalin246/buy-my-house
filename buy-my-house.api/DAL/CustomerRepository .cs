using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BuyMyHouse.Models;

namespace BuyMyHouse.DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;

        public CustomerRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task UpdateCustomerFinancialInfoAsync(int customerId, decimal? newIncome, int? newCreditScore)
        {
            var customer = await _context.Customers
                .Include(c => c.FinancialInformation)
                .FirstOrDefaultAsync(c => c.CustomerID == customerId);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }

            if (customer.FinancialInformation == null)
            {
                customer.FinancialInformation = new FinancialInformation
                {
                    CreditScore = newCreditScore,
                    Income = newIncome
                };
                _context.FinancialInformations.Add(customer.FinancialInformation);
            }
            else
            {
                customer.FinancialInformation.CreditScore = newCreditScore;
                customer.FinancialInformation.Income = newIncome;
            }

            await _context.SaveChangesAsync();
        }
    }
}
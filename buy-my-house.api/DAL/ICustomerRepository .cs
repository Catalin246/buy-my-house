using System.Collections.Generic;
using System.Threading.Tasks;
using BuyMyHouse.Models;

namespace BuyMyHouse.DAL
{
    public interface ICustomerRepository
    {
        Task UpdateCustomerFinancialInfoAsync(int customerId, decimal? newIncome, int? newCreditScore);
    }
}

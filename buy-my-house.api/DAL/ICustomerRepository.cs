using System.Collections.Generic;
using System.Threading.Tasks;
using BuyMyHouse.Models;

namespace BuyMyHouse.DAL
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
    }
}

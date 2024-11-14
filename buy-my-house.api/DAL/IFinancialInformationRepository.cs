using System.Collections.Generic;
using System.Threading.Tasks;
using BuyMyHouse.Models;

namespace BuyMyHouse.DAL
{
    public interface IFinancialInformationRepository
    {
        Task<IEnumerable<FinancialInformation>> GetAllAsync();
        Task<FinancialInformation?> GetByIdAsync(int id);
        Task AddAsync(FinancialInformation financialInformation);
        Task UpdateAsync(FinancialInformation financialInformation);
        Task DeleteAsync(int id);
    }
}

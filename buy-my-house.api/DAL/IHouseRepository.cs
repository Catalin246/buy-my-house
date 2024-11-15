using System.Collections.Generic;
using System.Threading.Tasks;
using BuyMyHouse.Models;

namespace BuyMyHouse.DAL
{
    public interface IHouseRepository
    {
        Task<IEnumerable<House>> GetAllAsync();
        // Task<House?> GetByIdAsync(int id);
        // Task AddAsync(House house);
        // Task UpdateAsync(House house);
        // Task DeleteAsync(int id);
    }
}


using System.Collections.Generic;
using System.Threading.Tasks;
using BuyMyHouse.Models;

namespace BuyMyHouse.DAL
{
    public interface IHouseRepository
    {
        Task<IEnumerable<House>> GetHousesInPriceRangeAsync(decimal? minPrice, decimal? maxPrice);
    }
}


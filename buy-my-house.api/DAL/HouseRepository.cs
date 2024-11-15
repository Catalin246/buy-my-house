using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BuyMyHouse.Models;

namespace BuyMyHouse.DAL
{
    public class HouseRepository : IHouseRepository
    {
        private readonly DatabaseContext _context;

        public HouseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<House>> GetAllAsync()
        {
            return await _context.Houses.ToListAsync();
        }

        // public async Task<House?> GetByIdAsync(int id)
        // {
        //     return await _context.Houses.FindAsync(id);
        // }

        // public async Task AddAsync(House house)
        // {
        //     await _context.Houses.AddAsync(house);
        //     await _context.SaveChangesAsync();
        // }

        // public async Task UpdateAsync(House house)
        // {
        //     _context.Houses.Update(house);
        //     await _context.SaveChangesAsync();
        // }

        // public async Task DeleteAsync(int id)
        // {
        //     var house = await _context.Houses.FindAsync(id);
        //     if (house != null)
        //     {
        //         _context.Houses.Remove(house);
        //         await _context.SaveChangesAsync();
        //     }
        // }
    }
}

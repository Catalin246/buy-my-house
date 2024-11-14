using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BuyMyHouse.Models;

namespace BuyMyHouse.DAL
{
    public class FinancialInformationRepository : IFinancialInformationRepository
    {
        private readonly DatabaseContext _context;

        public FinancialInformationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FinancialInformation>> GetAllAsync()
        {
            return await _context.FinancialInformations.ToListAsync();
        }

        public async Task<FinancialInformation?> GetByIdAsync(int id)
        {
            return await _context.FinancialInformations
                .FirstOrDefaultAsync(fi => fi.FinancialInformationID == id);
        }

        public async Task AddAsync(FinancialInformation financialInformation)
        {
            await _context.FinancialInformations.AddAsync(financialInformation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FinancialInformation financialInformation)
        {
            _context.FinancialInformations.Update(financialInformation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var financialInformation = await _context.FinancialInformations.FindAsync(id);
            if (financialInformation != null)
            {
                _context.FinancialInformations.Remove(financialInformation);
                await _context.SaveChangesAsync();
            }
        }
    }
}

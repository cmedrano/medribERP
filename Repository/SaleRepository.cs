using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;

        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _context.Sales
                .Include(s => s.Detail)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Detail)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByClientIdAsync(int clientId)
        {
            return await _context.Sales
                .Where(s => s.ClientId == clientId)
                .Include(s => s.Detail)
                .ToListAsync();
        }

        public async Task<Sale> AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            //await _context.SaveChangesAsync();
            return sale;
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Sales.AnyAsync(s => s.Id == id);
        }


    }
}

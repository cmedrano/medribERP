using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repositories
{
    public class PriceListRepository : IPriceListRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PriceListRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<PriceList>> GetAllAsync()
        {
            return await _context.PriceLists.ToListAsync();
        }

        public async Task<PriceList?> GetByIdAsync(int id)
        {
            return await _context.PriceLists.FindAsync(id);
        }

        public async Task AddAsync(PriceList entity)
        {
            _context.PriceLists.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PriceList entity)
        {
            _context.PriceLists.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.PriceLists.FindAsync(id);
            if (entity != null)
            {
                _context.PriceLists.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;
        public BrandRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BrandResponseDto>> GetAllBrandAsync()
        {
            try
            {
                var brands = await _context.Brand.ToListAsync();

                var brandDto = brands.Select(x => new BrandResponseDto()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return brandDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

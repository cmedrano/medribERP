using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _context;
        public ProductCategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductCategoryResponseDto>> GetAllProductCategoryAsync()
        {
            try
            {
                var productCategories = await _context.Product_Category.ToListAsync();

                var productCategoryDto = productCategories.Select(x => new ProductCategoryResponseDto()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

                return productCategoryDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

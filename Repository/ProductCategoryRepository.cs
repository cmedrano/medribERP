using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using System.ComponentModel.Design;

namespace PresupuestoMVC.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _context;
        public ProductCategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductCategoryResponseDto>> GetAllProductCategoryAsync(int companyId)
        {
            try
            {
                var productCategories = await _context.Product_Category
                    .Where(c => c.CompanyId == companyId)
                    .ToListAsync();

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

        public async Task<PaginatedResult<ProductCategoryResponseDto>> GetPagedAsync(int pageNumber, int pageSize, int companyId)
        {
            var query = _context.Product_Category
                .Where(a => a.CompanyId == companyId)
                .OrderBy(a => a.Name);

            var queryProductCategory = query.Select(p => new ProductCategoryResponseDto
            {
                Id = p.Id,
                Name = p.Name
            });

            var totalCount = await queryProductCategory.CountAsync();
            var items = await queryProductCategory
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<ProductCategoryResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ProductCategoryResponseDto> CreateProductCategoryAsync(CreateProductCategoryViewRequest productCategoryDto)
        {
            try
            {
                var RubroExiste = await _context.Product_Category
                .AnyAsync(r => r.Name == productCategoryDto.Nombre && r.CompanyId == productCategoryDto.CompanyId);

                if (RubroExiste)
                    throw new InvalidOperationException("El nombre del Rubro ya existe.");

                var productCategory = new ProductCategory()
                {
                    Name = productCategoryDto.Nombre,
                    CompanyId = productCategoryDto.CompanyId
                };

                _context.Product_Category.Add(productCategory);
                await _context.SaveChangesAsync();
                var createdRubro = await _context.Product_Category
                    .FirstOrDefaultAsync(r => r.Name == productCategoryDto.Nombre);

                return new ProductCategoryResponseDto
                {
                    Id = createdRubro.Id,
                    Name = createdRubro.Name,
                };
            }
            catch
            {
                throw new InvalidOperationException("error al crear el Rubro");
            }
        }

        public async Task Update(UpdateProductCategoryViewRequest model)
        {
            var rubroExiste = await _context.Product_Category
                .FirstOrDefaultAsync(r => r.Id == model.Id);

            if (rubroExiste == null)
                throw new Exception("No se encontró el rubro.");

            rubroExiste.Name = model.Nombre;

            await _context.SaveChangesAsync();
        }
    }
}

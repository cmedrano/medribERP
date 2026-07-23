using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;
using QuestPDF.Helpers;

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
                    Name = x.Name,
                    Code = x.Code,
                }).ToList();

                return brandDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PaginatedResult<BrandResponseDto>> GetAllBrandPageAsync(string searchBrands, int pagina, int tamañoPagina)
        {
            try
            {
                var query = _context.Brand.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchBrands))
                {
                    query = query.Where(x =>
                        EF.Functions.ILike(x.Name, $"%{searchBrands}%"));
                }

                var total = await query.CountAsync();

                var items = await query
                    .Skip((pagina - 1) * tamañoPagina)
                    .Take(tamañoPagina)
                    .ToListAsync();

                var brandDto = items.Select(x => new BrandResponseDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                }).OrderBy(x => x.Name)
                .ToList();

                return new PaginatedResult<BrandResponseDto>
                {
                    Items = brandDto,
                    TotalCount = total,
                    PageNumber = pagina,
                    PageSize = tamañoPagina,
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateBrandAsync(BrandRequestDto brand)
        {
            try
            {
                var existingBrand = await _context.Brand.FirstOrDefaultAsync(b => b.Code == brand.Codigo);

                if (existingBrand != null)
                    throw new Exception($"El codigo {brand.Codigo} ya existe y esta asociado a una marca.");

                var newBrand = new Brand
                {
                    Name = brand.Nombre,
                    Code = brand.Codigo,
                };

                await _context.Brand.AddAsync(newBrand);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudo crear la marca");
            }
        }

        public async Task DeleteBrandAsync(int id)
        {
            try
            {
                var brand = await _context.Brand.FindAsync(id);

                if (brand == null)
                    throw new Exception($"La marca con ID {id} no existe.");

                _context.Brand.Remove(brand);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudo eliminar la marca con ID {id}");
            }
        }

        public async Task UpdateBrandAsync(BrandUpdateRequestDto brand)
        {
            try
            {
                var existingBrand = await _context.Brand.FindAsync(brand.Id);

                if (existingBrand == null)
                    throw new Exception($"La marca con ID {brand.Id} no existe.");

                var codeInUse = await _context.Brand.AnyAsync(b => b.Code == brand.codigo && b.Id != brand.Id);
                if (codeInUse)
                    throw new Exception($"El codigo {brand.codigo} ya existe y esta asociado a otra marca.");

                existingBrand.Name = brand.nombre;
                existingBrand.Code = brand.codigo;

                _context.Brand.Update(existingBrand);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"No se pudo actualizar la marca con ID {brand.Id}");
            }
        }






    }
}

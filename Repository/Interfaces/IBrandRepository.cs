using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<BrandResponseDto>> GetAllBrandAsync(int companyId);
        Task<PaginatedResult<BrandResponseDto>> GetAllBrandPageAsync(string searchBrands, int pagina, int tamañoPagina, int companyId);
        Task<Brand> CreateBrandAsync(BrandRequestDto brand);
        Task DeleteBrandAsync(int id);
        Task UpdateBrandAsync(BrandUpdateRequestDto brand);
    }
}

using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponseDto>> GetAllBrandAsync();
        Task<PaginatedResult<BrandResponseDto>> GetAllBrandPageAsync(string searchBrands, int pagina, int tamañoPagina);
        Task CreateBrandAsync(BrandRequestDto brand);
        Task DeleteBrandAsync(int id);
        Task UpdateBrandAsync(BrandUpdateRequestDto brand);
    }
}

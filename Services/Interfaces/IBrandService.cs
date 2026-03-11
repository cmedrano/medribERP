using PresupuestoMVC.Models.DTOs;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponseDto>> GetAllBrandAsync();
    }
}

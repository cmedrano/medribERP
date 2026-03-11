using PresupuestoMVC.Models.DTOs;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<BrandResponseDto>> GetAllBrandAsync();
    }
}

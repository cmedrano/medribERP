using PresupuestoMVC.Models.DTOs;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<IEnumerable<ProductCategoryResponseDto>> GetAllProductCategoryAsync();
    }
}

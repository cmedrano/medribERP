using PresupuestoMVC.Models.DTOs;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategoryResponseDto>> GetAllProductCategoryAsync();
    }
}

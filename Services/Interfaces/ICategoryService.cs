using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(int companyId);
        Task<CategoryResponseDto> CreateAsync(CreateCategoryViewRequest CreateDto);
        Task<int> GetCategoriesCountAsync(int companyId);
        Task<CategoryResponseDto> GetCategoryByIdAsync(int companyId, int rubroId);
    }
}

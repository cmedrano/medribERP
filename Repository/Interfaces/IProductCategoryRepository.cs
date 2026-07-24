using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<IEnumerable<ProductCategoryResponseDto>> GetAllProductCategoryAsync(int companyId);
        Task<PaginatedResult<ProductCategoryResponseDto>> GetPagedAsync(int pageNumber, int pageSize, int companyId);
        Task<ProductCategoryResponseDto> CreateProductCategoryAsync(CreateProductCategoryViewRequest productCategoryDto);
    }
}

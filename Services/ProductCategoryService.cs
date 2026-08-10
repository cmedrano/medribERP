using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<IEnumerable<ProductCategoryResponseDto>> GetAllProductCategoryAsync(int companyId)
        {
            return await _productCategoryRepository.GetAllProductCategoryAsync(companyId);
        }

        public async Task<PaginatedResult<ProductCategoryResponseDto>> GetPagedAsync(int pageNumber, int pageSize, int companyId)
        {
            return await _productCategoryRepository.GetPagedAsync(pageNumber, pageSize, companyId);
        }

        public async Task<ProductCategoryResponseDto> CreateProductCategoryAsync(CreateProductCategoryViewRequest productCategoryDto)
        {
            return await _productCategoryRepository.CreateProductCategoryAsync(productCategoryDto);
        }

        public async Task UpdateProductCategoryAsync(UpdateProductCategoryViewRequest productCategoryDto)
        {
            await _productCategoryRepository.Update(productCategoryDto);
        }

        public async Task DeleteProductCategoryAsync(int id)
        {
            await _productCategoryRepository.DeleteProductCategory(id);
        }
    }
}

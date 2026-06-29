using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace PresupuestoMVC.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(int companyId)
        {
            return await _categoryRepository.GetAllCategoriesAsync(companyId);
        }
        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryViewRequest CreateDto)
        {
            int? rubroPadreId = CreateDto.RubroPadreId == 0
                                ? null
                                : CreateDto.RubroPadreId;

            var CategoryDto = new RubroType
            {
                nombreRubro = CreateDto.Rubro ?? CreateDto.SubCategory,
                RubroPadreId = rubroPadreId,
                CompanyId = CreateDto.CompanyId
            };
            return await _categoryRepository.CreateAsync(CategoryDto);
        }
        public async Task<int> GetCategoriesCountAsync(int companyId)
        {
            var totalCategories = await _categoryRepository.GetCategoriesCountAsync(companyId);
            return totalCategories;
        }

        public async Task<CategoryResponseDto> GetCategoryByIdAsync(int companyId, int rubroId)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(companyId);
            return categories.FirstOrDefault(c => c.Id == rubroId);
        }
    }
}

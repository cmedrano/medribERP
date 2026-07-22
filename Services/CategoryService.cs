using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Accounting.Data.DTOs;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace PresupuestoMVC.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        public CategoryService(ICategoryRepository categoryRepository, IActivityLogRepository activityLogRepository)
        {
            _categoryRepository = categoryRepository;
            _activityLogRepository = activityLogRepository;
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

            var result = await _categoryRepository.CreateAsync(CategoryDto);

            var resultActivity = await _activityLogRepository.LogAsync(new ActivityLogRequestDto
            {
                CompanyId = CreateDto.CompanyId,
                EntityType = "Category",
                EntityId = result.Id,
                Action = "CREATE",
                Description = $"Se creó el rubro {result.nombreRubro}"
            });

            return result;
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

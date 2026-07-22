using Humanizer;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;
using PresupuestoMVC.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using PresupuestoMVC.Areas.Accounting.Data.DTOs;
using PresupuestoMVC.Repository;

namespace PresupuestoMVC.Services
{
    public class PriceListService : IPriceListService
    {
        private readonly IPriceListRepository _priceListRepository;
        private readonly IActivityLogRepository _activityLogRepository;

        public PriceListService(IPriceListRepository priceListRepository, IActivityLogRepository activityLogRepository)
        {
            _priceListRepository = priceListRepository;
            _activityLogRepository = activityLogRepository;
        }

        public async Task<List<PriceList>> GetAllAsync(int companyId)
        {
            return await _priceListRepository.GetAllAsync(companyId);
        }

        public async Task<PaginatedResult<PriceList>> GetPagedAsync(int pageNumber, int pageSize, int companyId)
        {
            return await _priceListRepository.GetPagedAsync(pageNumber, pageSize, companyId);
        }

        public async Task<PriceList?> GetByIdAsync(int id)
        {
            return await _priceListRepository.GetByIdAsync(id);
        }

        public async Task CreateListAsync(CreatePriceListViewRequest dto)
        {
            PriceList priceList = new PriceList()
            {
                CompanyId = dto.CompanyId,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Activo = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _priceListRepository.AddListAsync(priceList);

            var ActivityDto = new ActivityLogRequestDto()
            {
                CompanyId = dto.CompanyId,
                EntityType = "PriceList",
                Action = "CREATE",
                Description = $"Se creó una nueva lista de precio {priceList.Nombre}"
            };
            await _activityLogRepository.LogAsync(ActivityDto);
        }

        public async Task UpdateAsync(UpdatePriceListViewRequest dto)
        {
            await _priceListRepository.UpdateAsync(dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _priceListRepository.DeleteAsync(id);
        }
    }
}
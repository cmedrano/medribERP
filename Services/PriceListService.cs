using Humanizer;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class PriceListService : IPriceListService
    {
        private readonly IPriceListRepository _priceListRepository;

        public PriceListService(IPriceListRepository priceListRepository)
        {
            _priceListRepository = priceListRepository;
        }

        public async Task<List<PriceList>> GetAllAsync()
        {
            return await _priceListRepository.GetAllAsync();
        }

        public async Task<PriceList?> GetByIdAsync(int id)
        {
            return await _priceListRepository.GetByIdAsync(id);
        }

        public async Task CreateListAsync(CreatePriceListViewRequest dto)
        {
            PriceList priceList = new PriceList()
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Activo = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _priceListRepository.AddListAsync(priceList);
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
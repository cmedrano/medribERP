using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class PriceListService : IPriceListService
    {
        private readonly IPriceListRepository _repository;

        public PriceListService(IPriceListRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PriceList>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PriceList?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(PriceList model)
        {
            await _repository.AddAsync(model);
        }

        public async Task UpdateAsync(PriceList model)
        {
            await _repository.UpdateAsync(model);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
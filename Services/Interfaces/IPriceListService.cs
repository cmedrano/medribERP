using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IPriceListService
    {
        Task<List<PriceList>> GetAllAsync();

        Task<PriceList?> GetByIdAsync(int id);

        Task CreateAsync(PriceList model);

        Task UpdateAsync(PriceList model);

        Task DeleteAsync(int id);

    }
}

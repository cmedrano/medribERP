using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IPriceListRepository
    {
        Task<List<PriceList>> GetAllAsync();
        Task<PriceList?> GetByIdAsync(int id);
        Task AddAsync(PriceList entity);
        Task UpdateAsync(PriceList entity);
        Task DeleteAsync(int id);
    }
}

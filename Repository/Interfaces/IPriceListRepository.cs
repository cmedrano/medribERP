using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IPriceListRepository
    {
        Task<List<PriceList>> GetAllAsync();
        Task<PriceList?> GetByIdAsync(int id);
        Task AddListAsync(PriceList priceList);
        Task UpdateAsync(UpdatePriceListViewRequest dto);
        Task DeleteAsync(int id);
    }
}

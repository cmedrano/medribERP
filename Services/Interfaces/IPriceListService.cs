using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IPriceListService
    {
        Task<List<PriceList>> GetAllAsync();
        Task<PriceList?> GetByIdAsync(int id);
        Task CreateListAsync(CreatePriceListViewRequest dto);
        Task UpdateAsync(UpdatePriceListViewRequest dto);
        Task DeleteAsync(int id);

    }
}

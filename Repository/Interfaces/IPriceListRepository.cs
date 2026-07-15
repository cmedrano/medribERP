using PresupuestoMVC.Models;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IPriceListRepository
    {
        Task<List<PriceList>> GetAllAsync(int companyId);
        Task<PriceList?> GetByIdAsync(int id);
        Task<PaginatedResult<PriceList>> GetPagedAsync(int pageNumber, int pageSize, int companyId);
        Task AddListAsync(PriceList priceList);
        Task UpdateAsync(UpdatePriceListViewRequest dto);
        Task DeleteAsync(int id);
    }
}

using PresupuestoMVC.Models;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IPriceListService
    {
        Task<List<PriceList>> GetAllAsync(int companyId);
        Task<PriceList?> GetByIdAsync(int id);
        Task<PaginatedResult<PriceList>> GetPagedAsync(int pageNumber, int pageSize, int companyId);
        Task CreateListAsync(CreatePriceListViewRequest dto);
        Task UpdateAsync(UpdatePriceListViewRequest dto);
        Task DeleteAsync(int id);

    }
}

using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IFacturacionService
    {
        Task<int> CreateSaleAsync(SaleRequestDTO request);
        Task<Sale> GetSaleByIdAsync(int id);
        Task<Company> GetCompanyInfoAsync(int id);
    }
}

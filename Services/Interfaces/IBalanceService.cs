using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IBalanceService
    {
        Task<Balance> GenerarBalanceAsync(IEnumerable<int> presupuestoIds, int mes, int anio, int companyId);
        Task<IEnumerable<Balance>> GetAllAsync(int companyId);
    }
}

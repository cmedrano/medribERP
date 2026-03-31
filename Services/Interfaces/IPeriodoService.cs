using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IPeriodoService
    {
        Task<List<Periodo>> GetAllPeriodosAsync();
    }
}

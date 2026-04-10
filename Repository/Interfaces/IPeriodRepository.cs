using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IPeriodRepository
    {
        Task<PeriodResponseDto> CreatePeriodAsync(PeriodoResumen period);
        Task UpdatePeriodAsync(PeriodoResumen periodo);

        Task<List<PeriodoResumen>> GetAllPeriodsAsync();
    }
}

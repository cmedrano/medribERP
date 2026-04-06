using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IPeriodRepository
    {
        Task<PeriodResponseDto> CreatePeriodAsync(Periodo period);
        Task UpdatePeriodAsync(Periodo periodo);
    }
}

using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IPeriodoService
    {
        Task<List<Periodo>> GetAllPeriodosAsync();
        Task<List<Year>> GetAllYearsAsync();
        Task<List<Month>> GetAllMonthsAsync();
        Task<bool> ExistsAsync(int yearId, int monthId);
        Task<PeriodResponseDto> CreatePeriodAsync(CreatePeriodViewRequest accountRequest);
        Task UpdatePeriodAsync(int id, CreatePeriodViewRequest periodRequest);
    }
}

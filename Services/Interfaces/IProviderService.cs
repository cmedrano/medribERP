using PresupuestoMVC.Models.DTOs;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IProviderService
    {
        Task<IEnumerable<ProviderResponseDto>> GetAllProviderAsync();
    }
}

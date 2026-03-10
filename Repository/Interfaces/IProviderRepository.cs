using PresupuestoMVC.Models.DTOs;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<ProviderResponseDto>> GetAllProviderAsync();
    }
}

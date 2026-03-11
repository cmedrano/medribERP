using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }
        public async Task<IEnumerable<ProviderResponseDto>> GetAllProviderAsync()
        {
            return await _providerRepository.GetAllProviderAsync();
        }
    }
}

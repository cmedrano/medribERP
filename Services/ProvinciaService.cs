using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ProvinciaService : IProvinciaService
    {
        private readonly IProvinciaRepository _provinciaRepository;

        public ProvinciaService(IProvinciaRepository provinciaRepository)
        {
            _provinciaRepository = provinciaRepository;
        }

        public async Task<List<Provincia>> ObtenerTodasAsync()
        {
            return await _provinciaRepository.ObtenerTodasAsync();
        }
    }
}

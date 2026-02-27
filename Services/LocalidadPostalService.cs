using PresupuestoMVC.Services.Interfaces;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services
{
    public class LocalidadPostalService : ILocalidadPostalService
    {
        private readonly ILocalidadPostalRepository _repository;

        public LocalidadPostalService(ILocalidadPostalRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LocalidadPostal>> ObtenerPorCodigoPostalAsync(string codigoPostal)
        {
            return await _repository.ObtenerPorCodigoPostalAsync(codigoPostal);
        }

        public async Task<LocalidadPostal?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal)
        {
            return await _repository.ObtenerPorCodigoPostalPrimeroAsync(codigoPostal);
        }
    }
}

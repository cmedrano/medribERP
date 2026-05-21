using PresupuestoMVC.Services.Interfaces;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services
{
    public class LocalidadService : ILocalidadService
    {
        private readonly ILocalidadRepository _repository;

        public LocalidadService(ILocalidadRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Localidad>> ObtenerPorCodigoPostalAsync(string codigoPostal)
        {
            return await _repository.ObtenerPorCodigoPostalAsync(codigoPostal);
        }

        public async Task<Localidad?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal)
        {
            return await _repository.ObtenerPorCodigoPostalPrimeroAsync(codigoPostal);
        }
    }
}

using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface ILocalidadRepository
    {
        Task<List<Localidad>> ObtenerPorCodigoPostalAsync(string codigoPostal);
        Task<Localidad?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal);
    }
}

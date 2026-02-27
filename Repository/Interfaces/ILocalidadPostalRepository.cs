using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface ILocalidadPostalRepository
    {
        Task<List<LocalidadPostal>> ObtenerPorCodigoPostalAsync(string codigoPostal);
        Task<LocalidadPostal?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal);
    }
}

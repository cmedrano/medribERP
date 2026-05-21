using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface ILocalidadService
    {
        Task<List<Localidad>> ObtenerPorCodigoPostalAsync(string codigoPostal);
        Task<Localidad?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal);
    }
}

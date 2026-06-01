using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface ILocalidadService
    {
        Task<List<Localidad>> ObtenerTodasAsync();
        Task<List<Localidad>> ObtenerPorCodigoPostalAsync(string codigoPostal);
        Task<Localidad?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal);
        Task<List<Localidad>> ObtenerPorProvinciaAsync(int provinciaId);
    }
}

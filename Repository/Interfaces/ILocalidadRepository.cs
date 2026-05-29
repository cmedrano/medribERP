using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface ILocalidadRepository
    {
        Task<List<Localidad>> ObtenerTodasAsync();
        Task<List<Localidad>> ObtenerPorCodigoPostalAsync(string codigoPostal);
        Task<Localidad?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal);
        Task<List<Localidad>> ObtenerPorProvinciaAsync(int provinciaId);
    }
}

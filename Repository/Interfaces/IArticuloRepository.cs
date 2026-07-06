using PresupuestoMVC.Areas.Ventas.ViewModels;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IArticuloRepository
    {
        Task<IEnumerable<Articulo>> ObtenerTodosActivosAsync();
        Task<Articulo> ObtenerPorIdAsync(int id);
        Task<Articulo> ObtenerPorCodigoAsync(string codigo);
        Task GuardarAsync(Articulo articulo, List<ArticulosPrecios> articulosPrecios);
        Task ActualizarAsync(Articulo articulo, List<ArticulosPrecios> articulosPrecios);
        Task EliminarAsync(int id);
        Task<int> ObtenerTotalAsync();
        Task<PaginatedResult<Articulo>> GetPagedAsync(int pageNumber, int pageSize);
    }
}

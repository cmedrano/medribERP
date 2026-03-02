using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IArticuloRepository
    {
        Task<IEnumerable<Articulo>> ObtenerTodosActivosAsync();
        Task<Articulo> ObtenerPorIdAsync(int id);
        Task<Articulo> ObtenerPorCodigoAsync(string codigo);
        Task GuardarAsync(Articulo articulo);
        Task ActualizarAsync(Articulo articulo);
        Task EliminarAsync(int id);
        Task<int> ObtenerTotalAsync();
    }
}

using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> ObtenerTodosAsync(int companyId);
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<Cliente?> ObtenerPorCuitAsync(string cuit);
        Task<Cliente?> ObtenerPorEmailAsync(string email);
        Task GuardarAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(int id);
        Task<int> ObtenerTotalAsync();
        Task<int> ObtenerCantidadDeClientesActivos(int companyId);
        Task<PaginacionRespuestaDto<Cliente>> ObtenerPaginadosAsync(int pageNumber, int pageSize, int companyId, string? searchNombre = null, string? searchFantasia = null);
    }
}

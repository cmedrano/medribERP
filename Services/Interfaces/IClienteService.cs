using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteResponseDTO?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<ClienteResponseDTO>> ObtenerTodosAsync(int companyId);
        Task<ClienteResponseDTO?> ObtenerPorCuitAsync(string cuit);
        Task<ClienteResponseDTO?> ObtenerPorEmailAsync(string email);
        Task<ClienteResponseDTO> GuardarAsync(CreateClienteViewRequest createDto);
        Task<ClienteResponseDTO> ActualizarAsync(UpdateClienteViewRequest updateDto);
        Task<bool> EliminarAsync(int id);
        Task<int> ObtenerTotalAsync();
        Task<int> GetActiveClientsCountAsync(int companyId);
        Task<PaginacionRespuestaDto<ClienteResponseDTO>> ObtenerPaginadosAsync(FiltroClienteViewRequest filtro, int pageNumber, int pageSize, int companyId);
    }
}

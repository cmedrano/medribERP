using PresupuestoMVC.Areas.Ventas.ViewModels;
using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IArticuloService
    {
        Task<IEnumerable<ArticuloResponseDTO>> ObtenerTodosActivosAsync();
        Task<ArticuloResponseDTO> ObtenerPorIdAsync(int id);
        Task<int> ObtenerTotalAsync();
        Task<ArticuloResponseDTO> CrearAsync(ArticuloCreateDTO createDto);
        Task<ArticuloResponseDTO> ActualizarAsync(ArticuloUpdateDTO updateDto);
        Task<bool> EliminarAsync(int id);
    }
}

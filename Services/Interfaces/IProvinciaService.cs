using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IProvinciaService
    {
        Task<List<Provincia>> ObtenerTodasAsync();
    }
}

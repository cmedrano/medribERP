using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IProvinciaRepository
    {
        Task<List<Provincia>> ObtenerTodasAsync();
    }
}

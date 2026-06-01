using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(int id);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<IEnumerable<Sale>> GetByClientIdAsync(int clientId);
        Task<Sale> AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

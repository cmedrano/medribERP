using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company> GetCompanyByIdAsync(int id);
    }
}

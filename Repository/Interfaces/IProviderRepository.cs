using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.ViewModels;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<ProviderResponseDto>> GetAllProviderAsync(int companyId);
        Task<PaginatedResult<ProviderResponseDto>> GetPagedAsync(int pageNumber, int pageSize, int companyId);
        Task<ProviderResponseDto> CreateSupplierAsync(CreateSupplierViewRequest supplierDto);
        Task<ProviderResponseDto> UpdateSupplierAsync(UpdateSupplierViewRequest supplierDto);
        Task<bool> DeleteSupplierAsync(int id);
    }
}

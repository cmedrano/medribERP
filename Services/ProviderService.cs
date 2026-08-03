using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }
        public async Task<IEnumerable<ProviderResponseDto>> GetAllProviderAsync(int companyId)
        {
            return await _providerRepository.GetAllProviderAsync(companyId);
        }
        public async Task<PaginatedResult<ProviderResponseDto>> GetPagedAsync(int pageNumber, int pageSize, int companyId)
        {
            return await _providerRepository.GetPagedAsync(pageNumber, pageSize, companyId);
        }
        public async Task<ProviderResponseDto> CreateSupplierAsync(CreateSupplierViewRequest supplierDto)
        {
            return await _providerRepository.CreateSupplierAsync(supplierDto);
        }
        public async Task<ProviderResponseDto> UpdateSupplierAsync(UpdateSupplierViewRequest supplierDto)
        {
            return await _providerRepository.UpdateSupplierAsync(supplierDto);
        }
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            return await _providerRepository.DeleteSupplierAsync(id);
        }
    }
}

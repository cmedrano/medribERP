using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandrRepository)
        {
            _brandRepository = brandrRepository;
        }
        public async Task<IEnumerable<BrandResponseDto>> GetAllBrandAsync(int companyId)
        {
            return await _brandRepository.GetAllBrandAsync(companyId);
        }

        public async Task<PaginatedResult<BrandResponseDto>> GetAllBrandPageAsync(string searchBrands, int pagina, int tamañoPagina, int companyId)
        {
            return await _brandRepository.GetAllBrandPageAsync(searchBrands, pagina, tamañoPagina, companyId);
        }

        public async Task CreateBrandAsync(BrandRequestDto brand)
        {
            await _brandRepository.CreateBrandAsync(brand);
        }

        public async Task DeleteBrandAsync(int id)
        {
            await _brandRepository.DeleteBrandAsync(id);
        }

        public async Task UpdateBrandAsync(BrandUpdateRequestDto brand)
        {
            await _brandRepository.UpdateBrandAsync(brand);
        }


    }
}

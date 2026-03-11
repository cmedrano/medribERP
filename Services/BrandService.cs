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
        public async Task<IEnumerable<BrandResponseDto>> GetAllBrandAsync()
        {
            return await _brandRepository.GetAllBrandAsync();
        }
    }
}

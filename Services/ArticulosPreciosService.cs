using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Areas.Ventas.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ArticulosPreciosService : IArticulosPreciosService
    {
        private readonly IArticulosPreciosRepository _articuloPreciosRepository;

        public ArticulosPreciosService(IArticulosPreciosRepository articulosPrecios)
        {
            _articuloPreciosRepository = articulosPrecios;
        }
        public async Task<List<PriceItemDto>> GetPreciosByArticuloAsync(int articuloId)
        {
            return await _articuloPreciosRepository.GetPreciosByArticuloAsync(articuloId);
        }
    }
}

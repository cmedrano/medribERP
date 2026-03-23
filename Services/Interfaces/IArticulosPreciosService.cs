using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Areas.Ventas.ViewModels;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface IArticulosPreciosService
    {
        Task<List<PriceItemDto>> GetPreciosByArticuloAsync(int articuloId);
    }
}

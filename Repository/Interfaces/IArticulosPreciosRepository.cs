using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Areas.Ventas.ViewModels;

namespace PresupuestoMVC.Repository.Interfaces
{
    public interface IArticulosPreciosRepository
    {
        Task<List<PriceItemDto>> GetPreciosByArticuloAsync(int articuloId);
    }
}

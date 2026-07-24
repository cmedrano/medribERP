using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;

namespace PresupuestoMVC.Areas.Ventas.ViewModels
{
    public class MarcaViewModel
    {
        public PaginatedResult<BrandResponseDto> Marcas { get; set; }
    }
}

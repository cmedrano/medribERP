using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Areas.Ventas.ViewModels.DTOs
{
    public class FacturacionViewModel
    {
        public IEnumerable<ClienteResponseDTO> Clientes { get; set; }
        public IEnumerable<ArticuloResponseDTO> Articulos { get; set; }
        public List<PriceList> ListasDePrecio { get; set; }
    }
}

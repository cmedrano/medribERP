using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Areas.Ventas.ViewModels.DTOs
{
    public class SaleRequestDTO
    {
        public int ClientId { get; set; }

        public string NameClient { get; set; }

        public string DNI { get; set; }

        public int PriceListId { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Total { get; set; }

        public int CompanyId { get; set; }

        public DateTime DateInserted { get; set; } = DateTime.UtcNow;

        // Navegación
        public List<SaleDetail> Detail { get; set; } = new List<SaleDetail>();
    }
}

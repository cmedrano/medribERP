using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;

namespace PresupuestoMVC.Models.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public string NameClient { get; set; }

        public string DNI { get; set; }

        public int? PriceListId { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Total { get; set; }

        public DateTime DateInserted { get; set; } = DateTime.UtcNow;

        // Navegación
        public List<SaleDetail> Detail { get; set; } = new List<SaleDetail>();
    }
}

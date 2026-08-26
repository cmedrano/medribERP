using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("sale")]
    public class Sale
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("client_id")]
        public int? ClientId { get; set; }

        [Column("company_id")]
        public int? CompanyId { get; set; }

        [Column("name_client")]
        public string NameClient { get; set; }

        [Column("dni")]
        public string DNI { get; set; }

        [Column("price_list_id")]
        public int? PriceListId { get; set; }

        [Column("sub_total")]
        public decimal Subtotal { get; set; }

        [Column("descuento")]
        public decimal Descuento { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("date_inserted")]
        public DateTime DateInserted { get; set; } = DateTime.UtcNow;

        // Navegación
        public List<SaleDetail> Detail { get; set; } = new List<SaleDetail>();
    }
}

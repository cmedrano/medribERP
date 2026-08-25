using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PresupuestoMVC.Models.Entities
{
    [Table("sale_detail")]
    public class SaleDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("sale_id")]
        public int SaleId { get; set; }
        
        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("code_item")]
        public string CodeItem { get; set; }

        [Column("name_item")]
        public string NameItem { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("unit_price")]
        public decimal PrecioUnitario { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        // Navegación
        public Sale Sale { get; set; }
    }
}

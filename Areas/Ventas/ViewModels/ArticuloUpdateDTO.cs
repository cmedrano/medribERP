using System.ComponentModel.DataAnnotations;

namespace PresupuestoMVC.Areas.Ventas.ViewModels
{
    public class ArticuloUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(100, ErrorMessage = "El código no puede exceder 100 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(255, ErrorMessage = "El nombre no puede exceder 255 caracteres")]
        public string Nombre { get; set; }

        [StringLength(50, ErrorMessage = "La unidad de medida no puede exceder 50 caracteres")]
        public string UnidadMedida { get; set; }

        [Required(ErrorMessage = "El rubro es obligatorio")]
        public int ProductCategoryId { get; set; }

        [Required(ErrorMessage = "La marca es obligatorio")]
        public int BrendId { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "El precio de compra es obligatorio")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "El precio de venta es obligatorio")]
        public decimal SalePrice { get; set; }

        //public decimal Margin { get; set; }

        public List<PriceItemDto>? Items { get; set; }
    }
}

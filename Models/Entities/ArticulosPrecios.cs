using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("articulos_precios")] 
    public class ArticulosPrecios
    {
        [Key]
        [Column("id")] 
        public int Id { get; set; }

        [Column("articulo_id")]
        public int ArticuloId { get; set; }

        [Column("lista_precio_id")]
        public int ListaPrecioId { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}

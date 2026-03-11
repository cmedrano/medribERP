using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PresupuestoMVC.Models.Entities
{
    [Table("product_category")]
    public class ProductCategory
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PresupuestoMVC.Models.Entities
{
    [Table("brand")]
    public class Brand
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}

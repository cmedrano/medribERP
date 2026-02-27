using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("provincias")]
    public class Provincia
    {
        [Key]
        [Column("id_provincia")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }
    }
}

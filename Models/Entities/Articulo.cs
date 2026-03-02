using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("articulos")] // Coincide con Neon
    public class Articulo
    {
        [Key]
        [Column("id")] // Mapeo explícito a minúsculas
        public int Id { get; set; }

        [Required]
        [StringLength(50)] // El DB es VARCHAR(50)
        [Column("codigo")]
        public string Codigo { get; set; }

        [Required]
        [StringLength(200)] // El DB es VARCHAR(200)
        [Column("nombre")]
        public string Nombre { get; set; }

        [StringLength(20)] // El DB es VARCHAR(20)
        [Column("unidad_medida")]
        public string UnidadMedida { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("created_at")] // Mapeo a snake_case
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("localidades_postal")]
    public class LocalidadPostal
    {
        [Key]
        [Column("id_cod_postal")]
        public int IdCodPostal { get; set; }

        [Required]
        [StringLength(10)]
        [Column("codigo_postal")]
        public string CodigoPostal { get; set; }

        [Required]
        [StringLength(100)]
        [Column("localidad")]
        public string Localidad { get; set; }

        [Column("id_provincia")]
        public int IdProvincia { get; set; }

        [ForeignKey("IdProvincia")]
        public virtual Provincia Provincia { get; set; }

    }
}
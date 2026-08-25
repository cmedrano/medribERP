using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("price_list")]
    public class PriceList
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Nombre { get; set; }

        [Column("active")]
        public bool Activo { get; set; }

        [Column("create_at")]
        public DateTime CreatedAt { get; set; }

        [Column("update_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("description")]
        public string? Descripcion { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }
    }
}

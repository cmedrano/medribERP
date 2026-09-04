using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("balance")]
    public class Balance
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("balance")]
        public decimal ValorBalance { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }

        [Column("month")]
        public int Mes { get; set; }

        [Column("year")]
        public int Anio { get; set; }
    }
}

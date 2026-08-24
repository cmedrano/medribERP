using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("account")]
    public class Cuenta
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("account_name")]
        public string nombreCuenta { get; set; }

        [Column("initial_balance")]
        public decimal SaldoInicial { get; set; }

        [Column("current_balance")]
        public decimal SaldoActual { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}

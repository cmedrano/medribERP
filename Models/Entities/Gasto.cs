using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("expenses")]
    public class Gasto
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("date")]
        public DateTime Fecha { get; set; }

        [Column("amount")]
        public decimal Monto { get; set; }

        [Column("note")]
        public string? Nota { get; set; }

        // Relaciones
        [Column("category_type_id")]
        public int RubroTypeId { get; set; }
        public RubroType RubroType { get; set; }

        [Column("account_id")]
        public int CuentaId { get; set; }
        public Cuenta Cuenta { get; set; }

        [Column("create_by_user_id")]
        public int CreateByUserId { get; set; }
        public User CreateByUser { get; set; }

        [Column("update_by_user_id")]
        public int? UpdateByUserId { get; set; }
        public User? UpdateByUser { get; set; }

        [Column("delete_by_user_id")]
        public int? DeleteByUserId { get; set; }
        public User? DeleteByUser { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }

        [Column("delete_date")]
        public DateTime? DeleteDate { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
        [Column("period_id")]
        public int? PeriodoId { get; set; }

        [ForeignKey(nameof(PeriodoId))]
        public PeriodoResumen? Periodo { get; set; }
    }
}

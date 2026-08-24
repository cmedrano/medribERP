using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("budget")]
    public class Budget
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("category_type_id")]
        public int? RubroTypeId { get; set; } // Foreign Key

        [Column("initial_value")]
        public decimal valorInicial { get; set; }

        [Column("value_spent")]
        public decimal ValorGastado { get; set; }

        [Column("month")]
        public int Mes { get; set; }

        [Column("year")]
        public int Anio { get; set; }

        [Column("create_by_user")]
        public User CreateByUser { get; set; }

        [Column("create_by_user_id")]
        public int CreateByUserId { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }

        [Column("delete_date")]
        public DateTime? DeleteDate { get; set; }

        [Column("update_by_user")]
        public User? UpdateByUser { get; set; }

        [Column("update_by_user_id")]
        public int? UpdateByUserId { get; set; }

        [Column("delete_by_user")]
        public User? DeleteByUser { get; set; }

        [Column("delete_by_user_id")]
        public int? DeleteByUserId { get; set; }

        // Navegación
        public RubroType tipoRubro { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
    }
}

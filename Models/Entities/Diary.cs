using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("diary")]
    public class Diary
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

        [Column("column_string_test")]
        public string? ColumnStringTest { get; set; }

        [Column("column_int_test")]
        public int ColumnIntTest { get; set; }

        [Column("column_test_dev")]
        public int ColumnTestDev { get; set; }
    }
}

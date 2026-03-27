using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("income_transfers")]
    public class Income
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("create_date")]
        public DateTime Date { get; set; }

        [Column("from_account_id")]
        public int? FromAccountId { get; set; }

        public Cuenta FromAccount { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("type_id")]
        public int TypeId { get; set; }

        [Column("to_account_id")]
        public int ToAccountId { get; set; }
        public Cuenta ToAccount { get; set; }
    }
}

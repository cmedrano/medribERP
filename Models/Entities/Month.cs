using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("months")]
    public class Month
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("month_number")]
        public int MonthNumber { get; set; }
    }
}
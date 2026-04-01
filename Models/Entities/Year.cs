using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("years")]
    public class Year
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("year")]
        public int YearValue { get; set; }
    }
}
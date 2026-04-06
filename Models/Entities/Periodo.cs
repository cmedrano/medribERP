using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("periods")]
    public class Periodo
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("ValorPresupuestado")]
        public int ValorPresupuestado { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("provider")]
    public class Provider
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}

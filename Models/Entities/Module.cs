using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("modules")]
    public class Module
    {
        [Key]
        [Column("module_id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PresupuestoMVC.Models.Entities
{
    [Table("areas_per_user")]
    public class AreasPerUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("module_id")]
        public int ModuleId { get; set; }

        public Module Module { get; set; }

    }
}

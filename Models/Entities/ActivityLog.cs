using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("activity_log")]
    public class ActivityLog
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("company_id")]
        public int CompanyId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Required]
        [Column("entity_type")]
        [StringLength(100)]
        public string EntityType { get; set; } = null!;

        [Column("entity_id")]
        public long? EntityId { get; set; }

        [Required]
        [Column("action")]
        [StringLength(30)]
        public string Action { get; set; } = null!;

        [Required]
        [Column("description")]
        public string Description { get; set; } = null!;

        [Column("old_values", TypeName = "jsonb")]
        public string? OldValues { get; set; }

        [Column("new_values", TypeName = "jsonb")]
        public string? NewValues { get; set; }

        [Column("ip_address")]
        [StringLength(45)]
        public string? IpAddress { get; set; }

        [Column("user_agent")]
        public string? UserAgent { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

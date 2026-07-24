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

        [Column("company_id")]
        public int CompanyId { get; set; }

        [Column("company")]
        public string Company { get; set; }

        [Column("code")]
        public string? Code { get; set; }
        
        [Column("phone")]
        public int? Phone { get; set; }
        
        [Column("address")]
        public string? Address { get; set; }

        [Column("email")]
        public string? Email { get; set; }
        
        [Column("responsible")]
        public string? Responsible { get; set; }
    }
}

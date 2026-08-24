using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("category_type")]
    public class RubroType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("category_name")]
        public string nombreRubro { get; set; }

        [Column("category_father_id")]
        public int? RubroPadreId { get; set; }
        public RubroType? RubroPadre { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
        //public bool EsSistema { get; set; }
        public ICollection<RubroType> SubRubros { get; set; } = new List<RubroType>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("articles")] // Coincide con Neon
    public class Articulo
    {
        [Key]
        [Column("id")] // Mapeo expl�cito a min�sculas
        public int Id { get; set; }

        [Required]
        [StringLength(50)] // El DB es VARCHAR(50)
        [Column("code")]
        public string? Codigo { get; set; }

        [Required]
        [StringLength(200)] // El DB es VARCHAR(200)
        [Column("name")]
        public string Nombre { get; set; }

        [StringLength(20)] // El DB es VARCHAR(20)
        [Column("unit_measure")]
        public string? UnidadMedida { get; set; }

        [Column("active")]
        public bool Activo { get; set; }

        [Column("created_at")] // Mapeo a snake_case
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("category_id")]
        public int ProductCategoryId { get; set; }

        [ForeignKey(nameof(ProductCategoryId))]
        public ProductCategory? ProductCategory { get; set; }

        [Column("brand_id")]
        public int BrendId { get; set; }

        [ForeignKey(nameof(BrendId))]
        public Brand? Brand { get; set; }

        [Column("provider_id")]
        public int ProviderId { get; set; }

        [ForeignKey(nameof(ProviderId))]
        public Provider? Provider { get; set; }

        [Column("purchase_price")]
        public decimal? PurchasePrice { get; set; }

        [Column("sale_price")]
        public decimal? SalePrice { get; set; }

        [Column("margin")]
        public decimal? Margin { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }
    }
}
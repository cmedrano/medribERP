using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    [Table("client")]
    public class Cliente
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        [Column("name")]
        public string Nombre { get; set; }

        [StringLength(50)]
        [Display(Name = "Tel�fono")]
        [Column("phone")]
        public string? Telefono { get; set; }

        [Display(Name = "Fecha de Registro")]
        [Column("date_registration")]
        public DateTime FechaRegistro { get; set; }

        [StringLength(255)]
        [Display(Name = "Domicilio")]
        [Column("address")]
        public string? Domicilio { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre")]
        [Column("city")]
        public string? Localidad { get; set; }

        [StringLength(100)]
        [Display(Name = "Provincia")]
        [Column("Province")]
        public string? Provincia { get; set; }

        [StringLength(20)]
        [Display(Name = "C�digo Postal")]
        [Column("zip_code")]
        public string? CodigoPostal { get; set; }

        [StringLength(200)]
        [Display(Name = "Email")]
        [Column("email")]
        public string? Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Celular")]
        [Column("mobile_phone")]
        public string? Celular { get; set; }

        [StringLength(20)]
        [Display(Name = "DNI")]
        [Column("dni")]
        public string? DNI { get; set; }

        [StringLength(20)]
        [Display(Name = "CUIT")]
        [Column("cuit")]
        public string? CUIT { get; set; }

        [Display(Name = "Activo")]
        [Column("active")]
        public bool Activo { get; set; } = true;

        [StringLength(100)]
        [Display(Name = "Fantas�a")]
        [Column("nick_name")]
        public string? Fantasia { get; set; }

        [StringLength(100)]
        [Display(Name = "CondicionDeVenta")]
        [Column("sale_condition")]
        public string? CondicionDeVenta { get; set; }

        [StringLength(100)]
        [Display(Name = "Categor�a")]
        [Column("category")]
        public string? Categoria { get; set; }

        [Display(Name = "Operaciones al Contado")]
        [Column("cash_pperations")]
        public bool OperacionesContado { get; set; } = false;

        [Display(Name = "Inhabilitado para Facturar")]
        [Column("is_billing_disabled")]
        public bool InhabilitadoFacturar { get; set; } = false;

        [Column("company_id")]
        public int CompanyId { get; set; }

        // FK
        [Column("lista_precio_id")]
        public int PriceListId { get; set; }

        // navegaci�n
        [ForeignKey(nameof(PriceListId))]
        public PriceList PriceList { get; set; }
    }
}

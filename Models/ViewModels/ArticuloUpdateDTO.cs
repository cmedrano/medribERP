using System.ComponentModel.DataAnnotations;

namespace PresupuestoMVC.Models.ViewModels
{
    public class ArticuloUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(100, ErrorMessage = "El código no puede exceder 100 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(255, ErrorMessage = "El nombre no puede exceder 255 caracteres")]
        public string Nombre { get; set; }

        [StringLength(50, ErrorMessage = "La unidad de medida no puede exceder 50 caracteres")]
        public string UnidadMedida { get; set; }
    }
}

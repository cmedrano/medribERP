using System.ComponentModel.DataAnnotations;

namespace PresupuestoMVC.Models.ViewModels
{
    public class RecoverViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un correo electrónico.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; } = string.Empty;
    }
}

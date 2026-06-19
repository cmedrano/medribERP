using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Areas.Ventas.ViewModels.DTOs
{
    public class PreviewPdfViewModel
    {
        public Sale Sale { get; set; }

        public ClienteResponseDTO Cliente { get; set; }

        public Company Company { get; set; }
    }
}

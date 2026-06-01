using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.Entities
{
    public class Localidad
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string CodigoPostal { get; set; } = string.Empty;

        public DateTime DateInserted { get; set; }

        public int ProvinciaId { get; set; }

        public Provincia Provincia { get; set; } = new Provincia();

    }
}
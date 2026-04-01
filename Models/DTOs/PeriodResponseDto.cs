using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.DTOs
{
    public class PeriodResponseDto
    {
        public DateTime Fecha { get; set; }

        public int ValorPresupuestado { get; set; }
    }
}

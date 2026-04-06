using System.ComponentModel.DataAnnotations.Schema;

namespace PresupuestoMVC.Models.DTOs
{
    public class PeriodResponseDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        public int ValorPresupuestado { get; set; }

        public int TotalGastos { get; set; }
    }
}

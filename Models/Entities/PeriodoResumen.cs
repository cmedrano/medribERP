namespace PresupuestoMVC.Models.Entities
{
    public class PeriodoResumen
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ValorPresupuestado { get; set; }
        public int TotalGastos { get; set; }
    }
}
namespace PresupuestoMVC.Models.ViewModels
{
    public class CreatePeriodViewRequest
    {
        public int YearId { get; set; }

        public int MonthId { get; set; }

        public int ValorPresupuestado { get; set; }
    }
}

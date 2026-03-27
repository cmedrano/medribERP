namespace PresupuestoMVC.Models.ViewModels
{
    public class CreateIncomeViewRequest
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }

    }
}

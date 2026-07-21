namespace PresupuestoMVC.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal SalesMonth { get; set; }
        public int ActiveClients { get; set; }
        public decimal UsedBudget { get; set; }
        public decimal AvailableBudget { get; set; }
        public int Users { get; set; }
        public int Clients { get; set; }
        public int ArticlesCount { get; set; }
        public int PriceListCount { get; set; }

    }
}

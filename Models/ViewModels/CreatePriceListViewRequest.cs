namespace PresupuestoMVC.Models.ViewModels
{
    public class CreatePriceListViewRequest
    {
        public string Nombre { get; set; }
        public int CompanyId { get; set; }
        public string? Descripcion { get; set; }
    }
}

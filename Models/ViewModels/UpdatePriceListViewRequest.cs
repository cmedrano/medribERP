namespace PresupuestoMVC.Models.ViewModels
{
    public class UpdatePriceListViewRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}

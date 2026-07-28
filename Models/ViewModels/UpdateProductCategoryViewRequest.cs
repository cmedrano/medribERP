namespace PresupuestoMVC.Models.ViewModels
{
    public class UpdateProductCategoryViewRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}

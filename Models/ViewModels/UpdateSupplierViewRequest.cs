namespace PresupuestoMVC.Models.ViewModels
{
    public class UpdateSupplierViewRequest
    { 
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string Empresa { get; set; }
        public int? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Mail { get; set; }
        public string? Responsable { get; set; }
    }
}

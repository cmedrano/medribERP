namespace PresupuestoMVC.Models.Entities
{
    public class SaleDetail
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public int ItemId { get; set; }

        public string CodeItem { get; set; }

        public string NameItem { get; set; }

        public int Quantity { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Total { get; set; }

        // Navegación
        public Sale Sale { get; set; }
    }
}

namespace PresupuestoMVC.Areas.Ventas.ViewModels.DTOs
{
    public class ArticuloResponseDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string UnidadMedida { get; set; }
        public bool Activo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ProductCategoryId { get; set; }
        public int BrendId { get; set; }
        public int ProviderId { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Margin { get; set; }
    }
}

namespace PresupuestoMVC.Models.ViewModels
{
    public class FiltroClienteViewRequest
    {
        public string? SearchNombre { get; set; }
        public string? SearchFantasia { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamañoPagina { get; set; } = 10;
    }
}

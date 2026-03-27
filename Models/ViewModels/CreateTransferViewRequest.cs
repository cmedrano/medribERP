namespace PresupuestoMVC.Models.ViewModels
{
    public class CreateTransferViewRequest
    {
        //public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public int AccountOriginId { get; set; }
        public int AccountDestinationId { get; set; }
    }
}

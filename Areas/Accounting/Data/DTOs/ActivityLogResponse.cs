namespace PresupuestoMVC.Areas.Accounting.Data.DTOs
{
    public class ActivityLogResponse
    {
        public int CompanyId { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

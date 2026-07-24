namespace PresupuestoMVC.Areas.Accounting.Data.DTOs
{
    public class ActivityLogRequestDto
    {
      public int CompanyId { get; set; }
      public int? UserId { get; set; }
      public string EntityType { get; set; }
      public long? EntityId { get; set; }
      public string Action { get; set; }
      public string Description { get; set; }
      public string? OldValues { get; set; }
      public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

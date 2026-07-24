namespace PresupuestoMVC.Models.DTOs
{
    public class ProviderResponseDto
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string? Code { get; set; }
        public int? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Responsible { get; set; }
    }
}

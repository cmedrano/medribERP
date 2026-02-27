namespace PresupuestoMVC.Models.DTOs
{
    public class ClienteResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }
        public string? Provincia { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Email { get; set; }
        public string? Celular { get; set; }
        public string? DNI { get; set; }
        public string? CUIT { get; set; }
        public string? Fantasia { get; set; }
        public string? Categoria { get; set; }
        public string? CondicionDeVenta { get; set; }
        public bool OperacionesContado { get; set; }
        public bool InhabilitadoFacturar { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}

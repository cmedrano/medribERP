using PresupuestoMVC.Enums;
using PresupuestoMVC.Models.ViewModels;
namespace PresupuestoMVC.Models.DTOs
{
    public class GastoResponseDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string? Nota { get; set; }
        public TransactionType TypeId { get; set; } = (TransactionType)1;
        public int ToAccountId { get; set; }

        // relaciones
        public int RubroTypeId { get; set; }
        public string RubroTypeNombre { get; set; }
        public string Tipo { get; set; } // "Ingreso" | "Transferencia"

        public int CuentaId { get; set; }
        public int CreateByUserId { get; set; }
        public string CuentaNombre { get; set; }
        public CreateGastoResult CreateGastoResult { get; set; }
    }
}

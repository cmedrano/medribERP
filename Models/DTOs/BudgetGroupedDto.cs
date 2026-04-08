using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Models.DTOs
{
    public class BudgetGroupedDto
    {
        public Budget Budget { get; set; }
        public List<Budget> SubBudget { get; set; }
    }
}

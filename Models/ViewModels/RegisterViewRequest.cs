using PresupuestoMVC.Enums;
using PresupuestoMVC.Models.Entities;

namespace PresupuestoMVC.Models.ViewModels
{
    public class RegisterViewRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public UserRol Role { get; set; }
        public List<Company> Companies { get; set; } = new List<Company>();
    }
}

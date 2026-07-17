using PresupuestoMVC.Models;

namespace PresupuestoMVC.Services.Interfaces
{
    public interface ITenantBrandingService
    {
        TenantBranding GetBranding(HttpContext httpContext);
    }
}

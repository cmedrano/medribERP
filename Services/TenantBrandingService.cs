using PresupuestoMVC.Models;
using PresupuestoMVC.Services.Interfaces;
using System.Net;

namespace PresupuestoMVC.Services
{
    public class TenantBrandingService : ITenantBrandingService
    {
        private readonly Dictionary<string, TenantBranding> _branding =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["avisur"] = new TenantBranding
                {
                    CompanyName = "Avisur",
                    BackgroundImage = "avisur.jpg",
                    Logo = "avisur-logo.png",
                    PrimaryColor = "#1B5E20"
                },

                ["cliente2"] = new TenantBranding
                {
                    CompanyName = "Cliente 2",
                    BackgroundImage = "cliente2.jpg",
                    Logo = "cliente2-logo.png",
                    PrimaryColor = "#1565C0"
                }
            };

        public TenantBranding GetBranding(HttpContext context)
        {
            var host = context.Request.Host.Host;

            var subdomain = GetSubdomain(host);

            if (subdomain != null &&
                _branding.TryGetValue(subdomain, out var branding))
            {
                return branding;
            }

            return new TenantBranding();
        }

        private static string? GetSubdomain(string host)
        {
            // localhost
            if (host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                return null;

            // 127.0.0.1
            if (IPAddress.TryParse(host, out _))
                return null;

            var parts = host.Split('.');

            if (parts.Length < 3)
                return null;

            return parts[0];
        }
    }
}

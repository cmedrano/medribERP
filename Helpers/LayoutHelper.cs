using Microsoft.AspNetCore.Http;

namespace PresupuestoMVC.Helpers
{
    /// <summary>
    /// Decide qué layout usar según el tipo de request.
    /// Cuando la navegación del menú lateral se hace por AJAX (ver wwwroot/js/site.js),
    /// el request llega con el header X-Requested-With: XMLHttpRequest y sólo se
    /// devuelve el fragmento de contenido (_AjaxLayout), sin el shell completo
    /// (sidebar, head, etc.), para evitar recargar toda la página.
    /// </summary>
    public static class LayoutHelper
    {
        private const string AjaxHeaderName = "X-Requested-With";
        private const string AjaxHeaderValue = "XMLHttpRequest";
        private const string AjaxLayout = "_AjaxLayout";
        private const string DefaultLayout = "~/Views/Shared/_Layout.cshtml";

        public static bool IsAjaxRequest(HttpContext context)
        {
            return context?.Request?.Headers[AjaxHeaderName].ToString() == AjaxHeaderValue;
        }

        public static string Resolve(HttpContext context, string fullLayout = DefaultLayout)
        {
            return IsAjaxRequest(context) ? AjaxLayout : fullLayout;
        }
    }
}

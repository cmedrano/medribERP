using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace PresupuestoMVC.Areas.Users.Controllers
{
    public class UserConfigController : Controller
    {
        [Area("Users")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Console.WriteLine($"SetLanguage llamado con culture={culture}, returnUrl={returnUrl}");

            if (!string.IsNullOrEmpty(culture))
            {
                // Guardar la cultura en una cookie
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
                );
            }

            // Redirigir a la página desde la que se envió el formulario
            return LocalRedirect(returnUrl ?? "/");
        }

        //[HttpPost]
        //public IActionResult SetLanguage(string culture, string returnUrl)
        //{
        //    Console.WriteLine($"SetLanguage ejecutado: culture={culture}, returnUrl={returnUrl}");

        //    if (!string.IsNullOrEmpty(culture))
        //    {
        //        Response.Cookies.Append(
        //            CookieRequestCultureProvider.DefaultCookieName,
        //            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        //            new CookieOptions
        //            {
        //                Expires = DateTimeOffset.UtcNow.AddYears(1),
        //                IsEssential = true,
        //                HttpOnly = false,
        //                SameSite = SameSiteMode.Lax,
        //                Secure = false
        //            }
        //        );
        //    }

        //    // Si returnUrl es null o vacío, redirige a la raíz
        //    if (string.IsNullOrEmpty(returnUrl))
        //        return RedirectToAction("Index", "Home");

        //    return LocalRedirect(returnUrl);
        //}
    }




}


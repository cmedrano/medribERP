using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Users.ViewModels;
using PresupuestoMVC.Data;
using PresupuestoMVC.Helpers;
using System.Security.Claims;

namespace PresupuestoMVC.Areas.Users.Controllers
{
    [Area("Users")]
    [Authorize]
    public class UserConfigController : Controller
    {
        private readonly AppDbContext _context;

        public UserConfigController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            var model = new UserConfigViewModel
            {
                Nombre = user.UserName,
                Email = user.UserEmail
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserConfigViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            // Validar que el email no exista
            bool emailExiste = await _context.Users
                .AnyAsync(x => x.UserEmail == model.Email && x.Id != userId);

            if (emailExiste)
            {
                ViewData["Error"] = "Ese email ya está siendo utilizado.";
                return View("Index", model);
            }

            user.UserName = model.Nombre;
            user.UserEmail = model.Email;

            await _context.SaveChangesAsync();

            ViewData["Success"] = "Los datos se actualizaron correctamente.";

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewData["Error"] = "Todos los campos de contraseña son obligatorios.";
                return View("Index");
            }

            if (newPassword != confirmPassword)
            {
                ViewData["Error"] = "La nueva contraseña y la confirmación no coinciden.";
                return View("Index");
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
            {
                ViewData["Error"] = "No se pudo identificar al usuario autenticado.";
                return View("Index");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                ViewData["Error"] = "Usuario no encontrado.";
                return View("Index");
            }

            if (!SecurityHelper.VerifyPassword(currentPassword, user.UserPasswordHash))
            {
                ViewData["Error"] = "La contraseña actual es incorrecta.";
                return View("Index");
            }

            user.UserPasswordHash = SecurityHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            ViewData["Success"] = "La contraseña se actualizó correctamente.";
            return View("Index");
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


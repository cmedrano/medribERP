using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly ITenantBrandingService _brandingService;
        private readonly ICompanyRepository _companyRepository;

        public LoginController(ILoginService loginService, ITenantBrandingService brandingService, ICompanyRepository companyRepository)
        {
            _loginService = loginService;
            _brandingService = brandingService;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si el usuario ya está autenticado, redirigir al home
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var branding = _brandingService.GetBranding(HttpContext);

            ViewBag.Branding = branding;
            return View();
        }

        // POST: /Login/Login - Procesa el formulario de login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewRequest loginRequest)
        {

            var branding = _brandingService.GetBranding(HttpContext);
            ViewBag.Branding = branding;
            if (!ModelState.IsValid)
            {
                return View(loginRequest);
            }

            try
            {
                var result = await _loginService.LoginAsync(loginRequest);

                // Guardar token en cookie (opcional) o session
                Response.Cookies.Append("X-Access-Token", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = result.Expiration
                });

                Response.Cookies.Append("X-Refresh-Token", result.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                // Redirigir al home después del login exitoso
                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(loginRequest);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error interno del servidor");
                return View(loginRequest);
            }
        }

        // GET: /Login/Register - Muestra el formulario de registro
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewRequest
            {
                Companies = await _companyRepository.GetAllCompanyAsync()
            };

            return View("Views/Register/Register.cshtml", model);
        }

        // POST: /Login/Register - Procesa el formulario de registro
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewRequest registerRequest)
        {
            var branding = _brandingService.GetBranding(HttpContext);
            ViewBag.Branding = branding;
            if (!ModelState.IsValid)
            {
                return View(registerRequest);
            }

            try
            {
                var result = await _loginService.RegisterAsync(registerRequest);

                if (result != null)
                {
                    // Redirigir al login después del registro exitoso
                    TempData["SuccessMessage"] = "Registro exitoso. Por favor inicia sesión.";
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(string.Empty, "Error en el registro");
                return View(registerRequest);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(registerRequest);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error interno del servidor");
                return View(registerRequest);
            }
        }

        // POST: /Login/Logout - Cerrar sesión
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Obtener refresh token de las cookies
            var refreshToken = Request.Cookies["X-Refresh-Token"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _loginService.RevokeRefreshTokenAsync(refreshToken);
            }

            // Limpiar cookies
            Response.Cookies.Delete("X-Access-Token");
            Response.Cookies.Delete("X-Refresh-Token");

            return RedirectToAction("Login", "Login");
        }

        // GET: /Login/Recover - Muestra el formulario de recuperación de contraseña
        [HttpGet]
        public IActionResult Recover()
        {
            return View("Views/Recover/Recover.cshtml");
        }


        // POST: /Login/Recover - Procesa el formulario de recuperación de contraseña
        [HttpPost]
        public async Task<IActionResult> Recover(RecoverViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _loginService.GetByEmailAsync(model);

                if (user != null)
                {
                    // Por seguridad no revelamos si el correo existe o no: siempre
                    // mostramos el mismo mensaje, pero solo enviamos el correo si el
                    // usuario existe realmente.
                    var token = await _loginService.GeneratePasswordResetTokenAsync(user);
                    var resetLink = Url.Action("ResetPassword", "Login", new { token }, Request.Scheme);

                    await _loginService.SendPasswordResetEmailAsync(user.UserEmail, resetLink);
                }

                TempData["SuccessMessage"] = "Si el correo ingresado está registrado, te enviamos las instrucciones para restablecer tu contraseña.";
                return RedirectToAction("Recover", "Login");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Views/Recover/Recover.cshtml", model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No se pudo enviar el correo de recuperación. Intenta nuevamente más tarde.");
                return View("Views/Recover/Recover.cshtml", model);
            }
        }

        // GET: /Login/ResetPassword - Muestra el formulario para crear una nueva contraseña
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Recover", "Login");

            var model = new ResetPasswordViewRequest { Token = token };
            return View("Views/Recover/ResetPassword.cshtml", model);
        }

        // POST: /Login/ResetPassword - Procesa el formulario de nueva contraseña
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewRequest model)
        {
            if (!ModelState.IsValid)
                return View("Views/Recover/ResetPassword.cshtml", model);

            try
            {
                await _loginService.ResetPasswordAsync(model);

                TempData["SuccessMessage"] = "Tu contraseña fue actualizada correctamente. Ya podés iniciar sesión.";
                return RedirectToAction("Login", "Login");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Views/Recover/ResetPassword.cshtml", model);
            }
        }





    }
}

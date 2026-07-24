using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services.Interfaces;
using System.Security.Claims;

namespace PresupuestoMVC.Controllers
{
    [Area("Ventas")]
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly ILocalidadService _localidadService;
        private readonly IPriceListService _priceListService;
        private readonly IProvinciaService _provinciaService;

        public ClientesController(
            IClienteService clienteService,
            ILocalidadService localidadService,
            IPriceListService priceListService,
            IProvinciaService provinciaService)
        {
            _clienteService = clienteService;
            _localidadService = localidadService;
            _priceListService = priceListService;
            _provinciaService = provinciaService;
        }

        public async Task<IActionResult> Index(string? searchNombre = null, string? searchFantasia = null, string? searchCliente = null, int page = 1, int tamañoPagina = 10)
        {
            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);

                if (page < 1)
                    page = 1;

                var filtro = new FiltroClienteViewRequest
                {
                    SearchNombre = searchNombre,
                    SearchFantasia = searchFantasia,
                    Pagina = page,
                    TamanioPagina = tamañoPagina
                };

                var resultado = await _clienteService.ObtenerPaginadosAsync(filtro, page, tamañoPagina, companyId);

                var priceList = await _priceListService.GetAllAsync(companyId);
                var provincia = await _provinciaService.ObtenerTodasAsync();
                ViewBag.PriceList = priceList;
                ViewBag.Provincia = provincia;

                if (!string.IsNullOrWhiteSpace(searchCliente))
                {
                    var filtered = resultado.Datos
                        .Where(x => x.Nombre.Contains(searchCliente, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    resultado.Datos = filtered;
                }

                ViewData["SearchCliente"] = searchCliente ?? "";
                ViewBag.Paginacion = resultado;
                ViewData["CurrentPage"] = page;
                ViewData["PageSize"] = tamañoPagina;
                ViewData["SearchNombre"] = searchNombre ?? "";
                ViewData["SearchFantasia"] = searchFantasia ?? "";

                // Si es una petición AJAX, retorna solo el partial view de la tabla
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_TablaClientesPartial", resultado);
                }

                return View(resultado);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCliente(CreateClienteViewRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Datos inválidos";
                    return RedirectToAction("Index");
                }
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                model.CompanyId = companyId;

                await _clienteService.GuardarAsync(model);
                TempData["Success"] = "Cliente registrado exitosamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente(UpdateClienteViewRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Datos inválidos";
                    return RedirectToAction("Index");
                }

                await _clienteService.ActualizarAsync(model);
                TempData["Success"] = "Cliente actualizado exitosamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["Error"] = "ID inválido";
                    return RedirectToAction("Index");
                }

                await _clienteService.EliminarAsync(id);
                TempData["Success"] = "Cliente eliminado exitosamente";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ValidarEmail(string email, int? clienteId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return Json(new { valido = true });
                }

                var clienteExistente = await _clienteService.ObtenerPorEmailAsync(email);

                // Si es para editar, verificar que el email no pertenezca a otro cliente
                if (clienteId.HasValue)
                {
                    var clienteActual = await _clienteService.ObtenerPorIdAsync(clienteId.Value);
                    if (clienteExistente != null && clienteExistente.Id != clienteId.Value)
                    {
                        return Json(new { valido = false, mensaje = "Este email ya está registrado en otro cliente activo" });
                    }
                }
                else
                {
                    // Si es para crear, verificar que no exista
                    if (clienteExistente != null)
                    {
                        return Json(new { valido = false, mensaje = "Este email ya está registrado en otro cliente activo" });
                    }
                }

                return Json(new { valido = true });
            }
            catch (Exception ex)
            {
                return Json(new { valido = false, mensaje = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerLocalidadesPorProvincia(int provinciaId)
        {
            var localidades = await _localidadService.ObtenerPorProvinciaAsync(provinciaId);

            return Json(localidades);
        }

        [HttpGet(Name = "ObtenerLocalidadPorCodigoPostal")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerLocalidadPorCodigoPostal(string codigoPostal)
        {
            if (string.IsNullOrWhiteSpace(codigoPostal))
            {
                return Json(new { success = false, mensaje = "Código postal inválido" });
            }

            try
            {
                var cpLimpio = System.Text.RegularExpressions.Regex.Replace(codigoPostal, @"\D", "");

                if (cpLimpio.Length != 4)
                {
                    return Json(new { success = false, mensaje = "El código postal debe tener 4 dígitos" });
                }

                // Buscar en la base de datos local
                var localidades = await _localidadService.ObtenerPorCodigoPostalAsync(cpLimpio);

                if (localidades == null || localidades.Count == 0)
                {
                    return Json(new { success = false, mensaje = "No se encontraron localidades para este código postal" });
                }

                // Obtener nombre de provincia desde ProvinciaService (O(1))
                //var idProvincia = localidades.FirstOrDefault()?.IdProvincia;
                //var provincia = _provinciaService.GetNombreProvincia(idProvincia);

                var localidadesList = localidades
                    .Select(x => x.Nombre)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .ToList();
                var provincia = localidades.FirstOrDefault()?.Provincia?.Nombre ?? "Provincia no especificada";

                if (localidadesList.Count == 0)
                {
                    return Json(new { success = false, mensaje = "No se encontraron localidades para este código postal" });
                }

                return Json(new
                {
                    success = true,
                    provincia = provincia,
                    localidades = localidadesList
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = $"Error: {ex.Message}" });
            }
        }
    }
}
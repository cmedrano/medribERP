using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Controllers
{
    [Authorize]
    public class ArticulosController : Controller
    {
        private readonly IArticuloService _articuloService;
        private readonly ILogger<ArticulosController> _logger;

        public ArticulosController(IArticuloService articuloService, ILogger<ArticulosController> logger)
        {
            _articuloService = articuloService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var articulos = await _articuloService.ObtenerTodosActivosAsync();
                var total = await _articuloService.ObtenerTotalAsync();

                ViewData["TotalArticulos"] = total;

                return View(articulos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los artículos: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarArticulo(ArticuloCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _articuloService.CrearAsync(model);
                TempData["Success"] = "Artículo creado exitosamente";
                return RedirectToAction("Index");
            }
            //catch (Exception ex)
            //{
            //    TempData["Error"] = "Error al crear el artículo: " + ex.Message;
            //    return View(model);
            //}
            catch (Exception ex)
            {
                // 1. Logueamos el error real para nosotros
                _logger.LogError(ex, "Error al intentar crear el artículo: {Nombre}", model.Nombre);

                // 2. Mensaje amigable para el usuario
                TempData["Error"] = "No se pudo procesar la solicitud. Si el problema persiste, contacte a soporte.";
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarArticulo(ArticuloUpdateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _articuloService.ActualizarAsync(model);
                TempData["Success"] = "Artículo actualizado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar el artículo: " + ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarArticulo(int id)
        {
            try
            {
                await _articuloService.EliminarAsync(id);
                TempData["Success"] = "Artículo eliminado exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar el artículo: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}

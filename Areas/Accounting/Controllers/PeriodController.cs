using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class PeriodController : Controller
    {
        private readonly IPeriodoService _periodoService;
        public PeriodController(IPeriodoService periodoService)
        {
            _periodoService = periodoService;
        }

        public async Task<IActionResult> Index()
        {

            try
            {
                var periodos = await _periodoService.GetAllPeriodosAsync();
                ViewBag.Periodos = periodos;
                ViewBag.Years = await _periodoService.GetAllYearsAsync();
                ViewBag.Months = await _periodoService.GetAllMonthsAsync();

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePeriodViewRequest periodRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarCombos();
                    return View("Index");
                }

                await _periodoService.CreatePeriodAsync(periodRequest);
                TempData["Success"] = "Periodo creado correctamente";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex) when (ex.Message == "DUPLICADO")
            {
                ModelState.AddModelError("MonthId", "este periodo de imputacion ya tiene un valor presupuestado");
                await CargarCombos();
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                await CargarCombos();
                return View("Index");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdatePeriod(CreatePeriodViewRequest periodRequest, int Id)
        {
            try
            {
                await _periodoService.UpdatePeriodAsync(Id, periodRequest);

                TempData["Success"] = "Periodo actualizado correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar: " + ex.Message;
                await CargarCombos();
                return View("Index");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CheckPeriodExists(int yearId, int monthId)
        {
            var exists = await _periodoService.ExistsAsync(yearId, monthId);
            return Json(new { exists });
        }

        private async Task CargarCombos()
        {
            // Cargamos los datos necesarios para la grilla y los dropdowns del modal
            ViewBag.Periodos = await _periodoService.GetAllPeriodosAsync();
            ViewBag.Years = await _periodoService.GetAllYearsAsync();
            ViewBag.Months = await _periodoService.GetAllMonthsAsync();
        }

    }
}

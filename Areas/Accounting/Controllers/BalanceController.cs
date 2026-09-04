using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Areas.Accounting.Data.Model;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services.Interfaces;
using System.Globalization;

namespace PresupuestoMVC.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class BalanceController : Controller
    {
        private readonly IBudgetService _budgetService;
        private readonly IBalanceService _balanceService;

        public BalanceController(IBudgetService budgetService, IBalanceService balanceService)
        {
            _budgetService = budgetService;
            _balanceService = balanceService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var today = DateTime.Now;
                var culture = new CultureInfo("es-AR");

                // Años: 2025 + 3 años 2025-2027
                var anios = Enumerable.Range(2025, 3).ToList();

                var meses = Enumerable.Range(1, 12)
                    .Select(m => new MesViewModel
                    {
                        Numero = m,
                        Nombre = culture.TextInfo.ToTitleCase(
                            culture.DateTimeFormat.GetMonthName(m).ToLower()
                        )
                    })
                    .ToList();

                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                var balances = await _balanceService.GetAllAsync(companyId);

                ViewBag.Meses = meses;
                ViewBag.Anios = anios;
                ViewBag.MesActual = today.Month;
                ViewBag.AnioActual = today.Year;
                ViewBag.Balances = balances;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPresupuestos(int? mes, int? anio)
        {
            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                var today = DateTime.Now;

                var filtro = new FiltroBudgetViewRequest
                {
                    Mes = mes ?? today.Month,
                    Anio = anio ?? today.Year,
                    Deficit = false,
                    RubroTypeId = null,
                    Pagina = 1,
                    TamañoPagina = 100
                };

                var resultado = await _budgetService.GetFiltradosAsync(filtro, 1, 100, companyId);

                var items = new List<object>();

                if (resultado?.Datos != null)
                {
                    foreach (var item in resultado.Datos)
                    {
                        var budget = item.Budget;
                        if (budget == null) continue;

                        items.Add(new
                        {
                            id = budget.Id,
                            rubro = budget.tipoRubro?.nombreRubro,
                            valorInicial = budget.valorInicial,
                            valorGastado = budget.ValorGastado,
                            disponible = budget.valorInicial - budget.ValorGastado,
                            mes = budget.Mes,
                            anio = budget.Anio
                        });

                        if (item.SubBudget != null)
                        {
                            foreach (var sub in item.SubBudget)
                            {
                                items.Add(new
                                {
                                    id = sub.Id,
                                    rubro = "- " + sub.tipoRubro?.nombreRubro,
                                    valorInicial = sub.valorInicial,
                                    valorGastado = sub.ValorGastado,
                                    disponible = sub.valorInicial - sub.ValorGastado,
                                    mes = sub.Mes,
                                    anio = sub.Anio
                                });
                            }
                        }
                    }
                }

                return Json(items);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerarBalance(List<int> presupuestoIds, int mes, int anio)
        {
            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                var balance = await _balanceService.GenerarBalanceAsync(presupuestoIds, mes, anio, companyId);

                TempData["Success"] = $"Se generó el balance correctamente (Total: {balance.ValorBalance:C0})";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}

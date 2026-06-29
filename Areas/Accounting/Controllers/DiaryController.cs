using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services.Interfaces;
using PresupuestoMVC.Services;

namespace PresupuestoMVC.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class DiaryController : Controller
    {
        private readonly IDiaryService _diaryService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;
        private readonly IGastoService _gastoService;
        private readonly IPeriodoService _periodoService;

        public DiaryController(IDiaryService diaryService, ICategoryService categodyService, IAccountService accountService, IGastoService gastoService, IPeriodoService periodoService)
        {
            _diaryService = diaryService;
            _categoryService = categodyService;
            _accountService = accountService;
            _gastoService = gastoService;
            _periodoService = periodoService;
        }
        public async Task<IActionResult> Index(int? rubroTypeId = null, int? cuentaId = null, int pagina = 1, int tamañoPagina = 10, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                // Cargar datos para los dropdowns
                var rubros = await _categoryService.GetAllCategoriesAsync(companyId);
                var cuentas = await _accountService.GetAllAccountAsync(companyId);
                var totalGsstos = await _gastoService.GetGastosCountAsync(companyId);
                var totalGsstosporMes = await _gastoService.GetGastosCountByMonthAsync(companyId);
                var periodos = await _periodoService.GetAllPeriodosAsync();

                // Crear filtro para el servicio
                var filtro = new FiltroGastoViewRequest
                {
                    RubroTypeId = rubroTypeId,
                    CuentaId = cuentaId,
                    FechaDesde = fechaDesde,
                    FechaHasta = fechaHasta,
                    Pagina = pagina,
                    TamañoPagina = tamañoPagina
                };

                // Obtener datos paginados y filtrados
                var resultadoPaginado = await _gastoService.GetFiltradosAsync(filtro, pagina, tamañoPagina, companyId);

                var hoy = DateTime.Today;
                var fechaUnaSemana = DateTime.UtcNow.Date.AddDays(-7);
                var gastoPorMes = await _gastoService.ObtenerGastoPorFecha(hoy);
                var gastoPorSemana = await _gastoService.ObtenerGastoPorFecha(fechaUnaSemana);
                
                // Pasar datos a la vista
                ViewBag.Rubros = rubros;
                ViewBag.Cuentas = cuentas;
                ViewBag.Periodos = periodos;
                ViewBag.FechaDesde = fechaDesde?.ToString("yyyy-MM-dd");
                ViewBag.FechaHasta = fechaHasta?.ToString("yyyy-MM-dd");
                ViewBag.Data = resultadoPaginado.Datos;
                ViewBag.Paginacion = resultadoPaginado;
                ViewBag.FiltroRubroId = rubroTypeId;
                ViewBag.FiltroCuentaId = cuentaId;
                ViewBag.PaginaActual = pagina;
                ViewBag.TamañoPagina = tamañoPagina;
                ViewBag.TotalGastos = totalGsstos;
                ViewBag.itemsCount = resultadoPaginado.Datos.Count();
                ViewBag.GastoTotalMes = gastoPorMes;
                ViewBag.GastoTotalSemana = gastoPorSemana;
                ViewBag.TotalGsstosporMes = totalGsstosporMes;

                return View();
            }
            catch (Exception ex)
            {
                // Manejar error y redirigir a página de error
                TempData["Error"] = "Error al cargar los datos: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }
    }
}

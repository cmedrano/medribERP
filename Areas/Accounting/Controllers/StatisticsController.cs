using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class StatisticsController : Controller
    {
        private readonly ICategoryService _categoryService;

        public StatisticsController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var rubros = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Rubros = rubros;
            return View();
        }
    }
}

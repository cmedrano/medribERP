using Microsoft.AspNetCore.Mvc;

namespace PresupuestoMVC.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

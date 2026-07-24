using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Controllers
{
    [Area("Ventas")]
    public class PriceListController : Controller
    {
        private readonly IPriceListService _priceListService;

        public PriceListController(IPriceListService priceListService)
        {
            _priceListService = priceListService;
        }

        public async Task<IActionResult> Index(string? searchNombre, int pagina = 1, int tamañoPagina = 10)
        {
            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                var resultadoPaginado = await _priceListService.GetPagedAsync(pagina, tamañoPagina, companyId);
                var model = await _priceListService.GetAllAsync(companyId);
                if (!string.IsNullOrWhiteSpace(searchNombre))
                {
                    var filteredItems = model
                        .Where(x => x.Nombre.Contains(searchNombre, StringComparison.OrdinalIgnoreCase))
                        .ToList(); 

                    resultadoPaginado.Items = filteredItems;
                    resultadoPaginado.TotalCount = filteredItems.Count;
                }

                ViewData["SearchNombre"] = searchNombre;
                ViewBag.Data = resultadoPaginado.Items;
                ViewBag.Paginacion = resultadoPaginado;
                ViewBag.ItemCounter = resultadoPaginado.Items.Count();
                ViewBag.PaginaActual = pagina;
                ViewBag.TamañoPagina = tamañoPagina;

                return View(resultadoPaginado.Items);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePriceListViewRequest dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
            dto.CompanyId = companyId;
            await _priceListService.CreateListAsync(dto);
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _priceListService.GetByIdAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditList(UpdatePriceListViewRequest dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _priceListService.UpdateAsync(dto);
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteList(int id)
        {
            await _priceListService.DeleteAsync(id);
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _priceListService.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            return View(item);
        }
    }
}
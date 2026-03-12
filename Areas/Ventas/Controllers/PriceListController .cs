using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var model = await _priceListService.GetAllAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePriceListViewRequest dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

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
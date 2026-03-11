using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;

public class PriceListController : Controller
{
    private readonly IPriceListService _service;

    public PriceListController(IPriceListService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _service.GetAllAsync();
        return View(data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PriceList model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _service.CreateAsync(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();

        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PriceList model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _service.UpdateAsync(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return View(item);
    }
}
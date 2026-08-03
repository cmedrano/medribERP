using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Areas.Ventas.Controllers
{
    [Area("Ventas")]
    public class SuppliersController : Controller
    {
        private readonly IProviderService _providerService;

        public SuppliersController(IProviderService providerService)
        {
            _providerService = providerService;
        }
        public async Task<IActionResult> Index(string? searchNombre, int pagina = 1, int tamañoPagina = 10)
        {
            int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
            var paginacion = await _providerService.GetPagedAsync(pagina, tamañoPagina, companyId);

            ViewBag.Paginacion = paginacion;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateSupplier(CreateSupplierViewRequest supplierDto)
        {
            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                supplierDto.CompanyId = companyId;
                var res = await _providerService.CreateSupplierAsync(supplierDto);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");

            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateSupplier(UpdateSupplierViewRequest supplierDto)
        {
            try
            {
                var res = await _providerService.UpdateSupplierAsync(supplierDto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");

            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                var res = await _providerService.DeleteSupplierAsync(id);
                if (!res)
                {
                    return RedirectToAction("Error", "Home");
                }
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

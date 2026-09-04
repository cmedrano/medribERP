using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Areas.Ventas.Controllers
{
    [Area("Ventas")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IActionResult> Index(string? searchNombre, int pagina = 1, int tamañoPagina = 10)
        {
            int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
            var paginacion = await _productCategoryService.GetPagedAsync(pagina, tamañoPagina, companyId);

            ViewBag.Paginacion = paginacion;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProductCategory(CreateProductCategoryViewRequest ProductCategoryDto)
        {
            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                ProductCategoryDto.CompanyId = companyId;
                var res = await _productCategoryService.CreateProductCategoryAsync(ProductCategoryDto);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, id = res.Id, nombre = res.Name });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, error = ex.Message });
                }

                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarProductCategory(UpdateProductCategoryViewRequest request)
        {

            try
            {
                await _productCategoryService.UpdateProductCategoryAsync(request);

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
        public async Task<IActionResult> borrarProductCategory(int id)
        {
            try
            {
                await _productCategoryService.DeleteProductCategoryAsync(id);

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

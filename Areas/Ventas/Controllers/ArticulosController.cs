using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Ventas.ViewModels;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Areas.Ventas.Controllers
{
    [Area("Ventas")]
    [Authorize]
    public class ArticulosController : Controller
    {
        private readonly IArticuloService _articuloService;
        private readonly IProviderService _providerService;
        private readonly IBrandService _brandService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly ILogger<ArticulosController> _logger;
        private readonly IPriceListService _priceListService;
        private readonly IArticulosPreciosService _articulosPreciosService;

        public ArticulosController(
            IArticuloService articuloService,
            IProviderService providerService,
            IBrandService brandService,
            IProductCategoryService productCategory,
            ILogger<ArticulosController> logger,
            IPriceListService priceListService,
            IArticulosPreciosService articulosPreciosService)
        {
            _articuloService = articuloService;
            _providerService = providerService;
            _brandService = brandService;
            _productCategoryService = productCategory;
            _logger = logger;
            _priceListService = priceListService;
            _articulosPreciosService = articulosPreciosService;
        }

        public async Task<IActionResult> Index(string? searchArticulo, int pagina = 1, int tamañoPagina = 10)
        {
            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                var articulos = await _articuloService.ObtenerTodosActivosAsync(companyId);
                var total = await _articuloService.ObtenerTotalAsync();
                var providers = await _providerService.GetAllProviderAsync(companyId);
                var brands = await _brandService.GetAllBrandAsync();
                var productCategories = await _productCategoryService.GetAllProductCategoryAsync();
                var priceList = await _priceListService.GetAllAsync(companyId);
                var resultadoPaginado = await _articuloService.GetPagedAsync(pagina, tamañoPagina, companyId);
            
                if (!string.IsNullOrWhiteSpace(searchArticulo))
                {
                    var filtrado = resultadoPaginado.Items.Where(x => x.Nombre.Contains(searchArticulo, StringComparison.OrdinalIgnoreCase)).ToList();
                    resultadoPaginado.Items = filtrado;
                }

                ViewData["SearchArticulo"] = searchArticulo;


                ViewBag.Data = resultadoPaginado.Items;
                ViewBag.Paginacion = resultadoPaginado;
                ViewBag.ItemCounter = resultadoPaginado.Items.Count();
                ViewBag.PaginaActual = pagina;
                ViewBag.TamañoPagina = tamañoPagina;

                ViewData["TotalArticulos"] = total;

                ViewBag.Articulos = resultadoPaginado.Items;
                ViewBag.ProductCategories = productCategories;
                ViewBag.Brands = brands;
                ViewBag.Providers = providers;
                ViewBag.PriceList = priceList;

                return View(articulos);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los artículos: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarArticulo(ArticuloCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                model.CompanyId = companyId;

                await _articuloService.CrearAsync(model);
                TempData["Success"] = "Artículo creado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // 1. Logueamos el error real para nosotros
                _logger.LogError(ex, "Error al intentar crear el artículo: {Nombre}", model.Nombre);

                // 2. Mensaje amigable para el usuario
                TempData["Error"] = "No se pudo procesar la solicitud. Si el problema persiste, contacte a soporte.";
                return RedirectToAction("Index");

            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarArticulo(ArticuloUpdateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                await _articuloService.ActualizarAsync(model);
                TempData["Success"] = "Artículo actualizado exitosamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
                //TempData["Error"] = "Error al actualizar el artículo: " + ex.Message;
                //return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarArticulo(int id)
        {
            try
            {
                await _articuloService.EliminarAsync(id);
                TempData["Success"] = "Artículo eliminado exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar el artículo: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetPreciosByArticulo(int articuloId)
        {
            var precios = await _articulosPreciosService.GetPreciosByArticuloAsync(articuloId);
            return Json(precios);
        }
    }
}

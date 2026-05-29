using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Areas.Ventas.Controllers
{
    [Area("Ventas")]
    public class FacturacionController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IPriceListService _priceListService;
        private readonly IArticuloService _articuloService;

        public FacturacionController(IClienteService clienteService, IPriceListService priceListService, IArticuloService articuloService)
        {
            _clienteService = clienteService;
            _priceListService = priceListService;
            _articuloService = articuloService;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();
            var articulos = await _articuloService.ObtenerTodosActivosAsync();
            var listPrice = await _priceListService.GetAllAsync();

            var viewModel = new FacturacionViewModel
            {
                Clientes = clientes,
                Articulos = articulos,
                ListasDePrecio = listPrice
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_FacturacionContent", viewModel);
            }
            return View(viewModel);
        }

        public class FacturacionViewModel
        {
            public IEnumerable<ClienteResponseDTO> Clientes { get; set; }
            public IEnumerable<ArticuloResponseDTO> Articulos { get; set; }
            public List<PriceList> ListasDePrecio { get; set; }
        }
    }
}

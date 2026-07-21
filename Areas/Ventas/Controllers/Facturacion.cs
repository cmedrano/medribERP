using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Areas.Ventas.ViewModels;
using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Areas.Ventas.Views.Facturacion.PDF;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Areas.Ventas.Controllers
{
    [Area("Ventas")]
    public class FacturacionController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IPriceListService _priceListService;
        private readonly IArticuloService _articuloService;
        private readonly IFacturacionService _facturacionService;


        public FacturacionController(IClienteService clienteService, IPriceListService priceListService, IArticuloService articuloService, IFacturacionService facturacionService)
        {
            _clienteService = clienteService;
            _priceListService = priceListService;
            _articuloService = articuloService;
            _facturacionService = facturacionService;
        }

        public async Task<IActionResult> Index()
        {
            int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
            var clientes = await _clienteService.ObtenerTodosAsync(companyId);
            var articulos = await _articuloService.ObtenerTodosActivosAsync(companyId);
            var listPrice = await _priceListService.GetAllAsync(companyId);

            var viewModel = new FacturacionViewModel
            {
                Clientes = clientes,
                Articulos = articulos,
                ListasDePrecio = listPrice
            };


            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] SaleRequestDTO request)
        {
            try
            {
                if (request == null)
                    return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

                var result = await _facturacionService.CreateSaleAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> ObtenerFacturasRecientes()
        {
            try
            {
                var ventas = await _facturacionService.GetRecentSalesAsync();

                var result = ventas.Select(v => new
                {
                    id = v.Id,
                    cliente = v.NameClient,
                    dni = v.DNI,
                    fecha = v.DateInserted.ToString("dd/MM/yyyy HH:mm"),
                    total = v.Total,
                    detalle = v.Detail.Select(d => new
                    {
                        articulo = d.NameItem,
                        cantidad = d.Quantity,
                        precioUnitario = d.PrecioUnitario,
                        total = d.Total
                    })
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PreviewPdf(int id)
        {
            try
            {
                var sale = await _facturacionService.GetSaleByIdAsync(id);
                var client = await _clienteService.ObtenerPorIdAsync((int)sale.ClientId);
                var company = await _facturacionService.GetCompanyInfoAsync(client.CompanyId);

                var ViewModel = new PreviewPdfViewModel
                {
                    Sale = sale,
                    Cliente = client,
                    Company = company
                };

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DescargarPdf(int id)
        {
            var sale = await _facturacionService.GetSaleByIdAsync(id);
            var client = await _clienteService.ObtenerPorIdAsync((int)sale.ClientId);
            var company = await _facturacionService.GetCompanyInfoAsync(2);

            var pdf = FacturaPdfGenerator.Generate(sale, client, company);

            return File(
                pdf,
                "application/pdf",
                $"factura-{sale.Id}.pdf"
            );
        }

    }
}

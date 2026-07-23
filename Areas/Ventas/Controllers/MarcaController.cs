using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Areas.Ventas.ViewModels;
using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Areas.Ventas.Controllers
{
    [Area("Ventas")]
    public class MarcaController : Controller
    {
        private readonly IBrandService _service;

        public MarcaController(IBrandService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string? searchBrands, int pageNumber = 1, int tamañoPagina = 10)
        {
            try
            {
                var marcasPaginadas = await _service.GetAllBrandPageAsync(searchBrands, pageNumber, tamañoPagina);

                var viewModel = new MarcaViewModel
                {
                    Marcas = marcasPaginadas
                };

                ViewData["SearchBrands"] = searchBrands;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
        }

        [HttpPost]

        public async Task<IActionResult> Crear([FromBody] BrandRequestDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

                await _service.CreateBrandAsync(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _service.DeleteBrandAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] BrandUpdateRequestDto request)
        {
            try
            {
                await _service.UpdateBrandAsync(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

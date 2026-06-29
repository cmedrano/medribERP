using Microsoft.AspNetCore.Mvc;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PresupuestoMVC.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class ImportFile : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICategoryService _categoryService;
        private readonly IGastoService _gastoService;

        public ImportFile(IAccountService accountService, ICategoryService categoryService, IGastoService gastoService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _gastoService = gastoService;
        }
        public async Task<IActionResult> Index()
        {
            int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
            var accounts = await _accountService.GetAllAccountAsync(companyId);
            var categories = await _categoryService.GetAllCategoriesAsync(companyId);

            ViewBag.Rubros = categories;
            ViewBag.Cuentas = accounts;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateImport([FromBody] List<CreateGastoViewRequest> items)
        {
            if (items == null || !items.Any())
            {
                return BadRequest(new { success = false, message = "No se recibieron registros para importar." });
            }

            try
            {
                int userId = int.Parse(
                     User.FindFirstValue(ClaimTypes.NameIdentifier)
                );
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);

                foreach (var item in items)
                {
                    item.CreateByUserId = userId;
                    item.CompanyId = companyId;
                    item.CreateDate = DateTime.UtcNow;
                }

                // Validar todos los items ANTES de hacer inserciones
                var validationErrors = new List<string>();
                foreach (var item in items)
                {
                    var validationResult = await ValidateGastoItemAsync(item);
                    if (!validationResult.IsValid)
                    {
                        validationErrors.Add(validationResult.ErrorMessage);
                    }
                }

                // Si hay errores, retornar sin insertar nada
                if (validationErrors.Any())
                {
                    return Json(new { success = false, message = validationErrors.First() });
                }

                // Si todas las validaciones pasaron, proceder con inserciones
                var responseList = new List<object>();

                foreach (var item in items)
                {
                    var result = await _gastoService.CreateAsync(item);
                    responseList.Add(new
                    {
                        success = true,
                        result = result
                    });
                }

                return Json(new { success = true, items = responseList });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<ValidationResult> ValidateGastoItemAsync(CreateGastoViewRequest item)
        {
            var account = await _accountService.GetAccountByIdAsync(item.CompanyId, item.CuentaId);
            if (account == null)
                return ValidationResult.Failure("Una de las cuentas no existe.");

            if (account.SaldoActual < item.Monto && !item.ForceNegativeBalance)
                return ValidationResult.Failure("Una de las cuentas no tiene saldo suficiente.");

            var category = await _categoryService.GetCategoryByIdAsync(item.CompanyId, item.RubroTypeId);
            if (category == null)
                return ValidationResult.Failure("Uno de los rubros no existe.");

            return ValidationResult.Success();
        }

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }

            public static ValidationResult Success() => new ValidationResult { IsValid = true };
            public static ValidationResult Failure(string message) => new ValidationResult { IsValid = false, ErrorMessage = message };
        }
    }
}

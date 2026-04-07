using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresupuestoMVC.Enums;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PresupuestoMVC.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {

            try
            {
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);
                var accounts = await _accountService.GetAllAccountAsync(companyId);
                var totalAccounts = await _accountService.GetAccountsCountAsync(companyId);

                ViewBag.Accounts = accounts;
                ViewBag.TotalAccounts = totalAccounts;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los datos: " + ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewRequest accountRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Datos inválidos";
                    return RedirectToAction("Index");
                }

                int companyId = int.Parse(User.FindFirst("CompanyId").Value);
                accountRequest.CompanyId = companyId;

                await _accountService.CreateAccountAsync(accountRequest);

                TempData["Success"] = "Cuenta creado correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateIncome(CreateIncomeViewRequest incomeRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Datos inválidos";
                    return RedirectToAction("Index");
                }

                await _accountService.CreateIncomeAsync(incomeRequest);

                TempData["Success"] = "se ingreso correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransfer(CreateTransferViewRequest transferRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Datos inválidos";
                    return RedirectToAction("Index");
                }

                await _accountService.CreateTransferAsync(transferRequest);

                TempData["Success"] = "la transferencia se hizo correctamente";
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

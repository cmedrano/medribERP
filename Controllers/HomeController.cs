using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Models;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;
using System.Diagnostics;

namespace PresupuestoMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly IGastoService _gastoService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;

        public HomeController(
            ILogger<HomeController> logger,
            IUserService userService,
            IBudgetService budgetService,
            IGastoService gastoService,
            ICategoryService categoryService,
            IAccountService accountService)
        {
            _logger = logger;
            _userService = userService;
            _budgetService = budgetService;
            _gastoService = gastoService;
            _categoryService = categoryService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var totalUsers = await _userService.GetUsersCountAsync();
            var totalBudgets = await _budgetService.GetBudgetCountAsync();
            var totalGsstos = await _gastoService.GetGastosCountAsync();
            var totalCategories = await _categoryService.GetCategoriesCountAsync();
            var totalAccounts = await _accountService.GetAccountsCountAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalBudgets = totalBudgets;
            ViewBag.TotalGsstos = totalGsstos;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalAccounts = totalAccounts;
            return View("Views/Home/Home.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

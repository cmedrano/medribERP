using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services;
using PresupuestoMVC.Services.Interfaces;
using System.Diagnostics;
using System.Security.Claims;

namespace PresupuestoMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IClienteService _clienteService;
        private readonly IBudgetService _budgetService;
        private readonly IGastoService _gastoService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;
        private readonly ISaleRepository _saleRepository;
        private readonly IArticuloService _articulosService;
        private readonly IPriceListService _priceListService;

        public HomeController(
            ILogger<HomeController> logger,
            IUserService userService,
            IClienteService clienteService,
            IBudgetService budgetService,
            IGastoService gastoService,
            ICategoryService categoryService,
            IAccountService accountService,
            ISaleRepository saleRepository,
            IArticuloService articuloService,
            IPriceListService priceListService)
        {
            _logger = logger;
            _userService = userService;
            _clienteService = clienteService;
            _budgetService = budgetService;
            _gastoService = gastoService;
            _categoryService = categoryService;
            _accountService = accountService;
            _saleRepository = saleRepository;
            _articulosService = articuloService;
            _priceListService = priceListService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                int companyId = int.Parse(User.FindFirst("CompanyId")?.Value);

                var salesMonth = await _saleRepository.GetSalesMonthAsync(userId);
                var activeClients = await _clienteService.GetActiveClientsCountAsync(companyId);
                var budgetSummary = await _budgetService.GetBudgetSummaryAsync(companyId);
                var totalUsers = await _userService.GetUsersCountAsync(companyId);
                var totalBudgets = await _budgetService.GetBudgetCountAsync(companyId);
                var totalGsstos = await _gastoService.GetGastosCountAsync(companyId);
                var totalCategories = await _categoryService.GetCategoriesCountAsync(companyId);
                var totalAccounts = await _accountService.GetAccountsCountAsync(companyId);
                var clients = await _clienteService.ObtenerTodosAsync(companyId);
                var articles = await _articulosService.ObtenerTodosActivosAsync(companyId);
                var priceList = await _priceListService.GetAllAsync(companyId);


                ViewBag.TotalUsers = totalUsers;
                ViewBag.TotalBudgets = totalBudgets;
                ViewBag.TotalGsstos = totalGsstos;
                ViewBag.TotalCategories = totalCategories;
                ViewBag.TotalAccounts = totalAccounts;
                ViewBag.Clients = clients;
                ViewBag.Articles = articles;
                ViewBag.PriceList = priceList;

                var modal = new DashboardViewModel
                {
                    SalesMonth = salesMonth,
                    ActiveClients = activeClients,
                    UsedBudget = budgetSummary.Used,
                    AvailableBudget = budgetSummary.Available
                };

                return View("Views/Home/Home.cshtml", modal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
            
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

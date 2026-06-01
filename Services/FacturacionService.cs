using Microsoft.AspNetCore.Http.HttpResults;
using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class FacturacionService : IFacturacionService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly AppDbContext _context;

        public FacturacionService(ISaleRepository saleRepository, ICompanyRepository companyRepository, AppDbContext context)
        {
            _saleRepository = saleRepository;
            _companyRepository = companyRepository;
            _context = context;
        }

        public async Task<int> CreateSaleAsync(SaleRequestDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            if (request.Detail == null || !request.Detail.Any())
                throw new Exception("La venta no tiene detalles");

            try
            {
                var sale = new Sale
                {
                    ClientId = request.ClientId,
                    NameClient = request.NameClient,
                    DNI = request.DNI,
                    PriceListId = request.PriceListId,

                    Subtotal = request.Subtotal,
                    Descuento = request.Descuento,
                    Total = request.Total,

                    Detail = request.Detail.Select(d => new SaleDetail
                    {
                        ItemId = d.ItemId,
                        CodeItem = d.CodeItem,
                        NameItem = d.NameItem,

                        Quantity = d.Quantity,
                        PrecioUnitario = d.PrecioUnitario,
                        Total = d.Total

                    }).ToList()
                };

                var result = await _saleRepository.AddAsync(sale);
                await _context.SaveChangesAsync();

                // Si todo salió bien, hago commit y devuelvo la respuesta exitosa
                await transaction.CommitAsync();

                return result.Id;
            }
            catch
            {
                // Si hubo errores en alguna de las etapas, hago rollback y devuelvo los errores
                await transaction.RollbackAsync();

                throw new Exception("Error al crear la venta");
            }
        }

        public async Task<IEnumerable<Sale>> GetRecentSalesAsync()
        {
            var sales = await _saleRepository.GetAllAsync();

            if (sales == null || !sales.Any())
                throw new Exception("No se encontraron ventas");

            return sales;
        }

        public async Task<Sale> GetSaleByIdAsync(int id)
        {
            if (id <= 0)
                throw new Exception("Id de venta no válido");

            var sale = await _saleRepository.GetByIdAsync(id);

            if (sale == null)
                throw new Exception("Venta no encontrada");

            return sale;
        }

        public async Task<Company> GetCompanyInfoAsync(int id)
        {
            if (id <= 0)
                throw new Exception("Id de empresa no válido");

            var company = await _companyRepository.GetCompanyByIdAsync(id);

            if (company == null)
                throw new Exception("Empresa no encontrada");

            return company;
        }



    }
}

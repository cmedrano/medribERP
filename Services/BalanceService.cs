using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly AppDbContext _context;

        public BalanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Balance> GenerarBalanceAsync(IEnumerable<int> presupuestoIds, int mes, int anio, int companyId)
        {
            var ids = (presupuestoIds ?? Enumerable.Empty<int>()).Distinct().ToList();

            if (!ids.Any())
                throw new Exception("Debe seleccionar al menos un presupuesto.");

            var budgets = await _context.Budget
                .Where(b => ids.Contains(b.Id) && b.CompanyId == companyId)
                .ToListAsync();

            if (!budgets.Any())
                throw new Exception("No se encontraron presupuestos válidos para generar el balance.");

            var totalBalance = budgets.Sum(b => b.valorInicial - b.ValorGastado);

            var balance = new Balance
            {
                ValorBalance = totalBalance,
                CompanyId = companyId,
                Mes = mes,
                Anio = anio
            };

            _context.Balances.Add(balance);
            await _context.SaveChangesAsync();

            return balance;
        }

        public async Task<IEnumerable<Balance>> GetAllAsync(int companyId)
        {
            return await _context.Balances
                .Where(b => b.CompanyId == companyId)
                .OrderByDescending(b => b.Anio)
                .ThenByDescending(b => b.Mes)
                .ThenByDescending(b => b.Id)
                .ToListAsync();
        }
    }
}

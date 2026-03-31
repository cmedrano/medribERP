using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class PeriodoService : IPeriodoService
    {
        private readonly AppDbContext _context;

        public PeriodoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Periodo>> GetAllPeriodosAsync()
        {
            return await _context.Periodos
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }
    }
}

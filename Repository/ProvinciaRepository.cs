using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repository
{
    public class ProvinciaRepository : IProvinciaRepository
    {
        private readonly AppDbContext _context;

        public ProvinciaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Provincia>> ObtenerTodasAsync()
        {
            return await _context.Provincias.ToListAsync();
        }
    }
}

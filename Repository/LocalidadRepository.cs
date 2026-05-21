using PresupuestoMVC.Data;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace PresupuestoMVC.Repository
{
    public class LocalidadRepository : ILocalidadRepository
    {
        private readonly AppDbContext _context;

        public LocalidadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Localidad>> ObtenerPorCodigoPostalAsync(string codigoPostal)
        {
            if (string.IsNullOrWhiteSpace(codigoPostal))
                return new List<Localidad>();

            // 1. Ejecutamos la consulta y esperamos el resultado (await)
            // Usamos .AsNoTracking() por performance si solo es para lectura
            var resultados = await _context.Localidades
                            .AsNoTracking() // Mejora el rendimiento para lectura
                            .Include(x => x.Provincia)
                            .Where(x => x.CodigoPostal.Trim() == codigoPostal.Trim())
                            .ToListAsync();

            // 2. Punto de interrupción (Breakpoint) aquí para inspeccionar 'resultados'
            return resultados;
        }

        public async Task<Localidad?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal)
        {
            if (string.IsNullOrWhiteSpace(codigoPostal))
                return null;

            return await _context.Localidades
                //.Include(x => x.Provincia)
                .Where(x => x.CodigoPostal.Trim() == codigoPostal.Trim())
                .FirstOrDefaultAsync();
        }
    }
}

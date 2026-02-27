using PresupuestoMVC.Data;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace PresupuestoMVC.Repository
{
    public class LocalidadPostalRepository : ILocalidadPostalRepository
    {
        private readonly AppDbContext _context;

        public LocalidadPostalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LocalidadPostal>> ObtenerPorCodigoPostalAsync(string codigoPostal)
        {
            if (string.IsNullOrWhiteSpace(codigoPostal))
                return new List<LocalidadPostal>();

            // 1. Ejecutamos la consulta y esperamos el resultado (await)
            // Usamos .AsNoTracking() por performance si solo es para lectura
            var resultados = await _context.localidades_postales
                            .AsNoTracking() // Mejora el rendimiento para lectura
                            .Include(x => x.Provincia)
                            .Where(x => x.CodigoPostal.Trim() == codigoPostal.Trim())
                            .ToListAsync();

            // 2. Punto de interrupción (Breakpoint) aquí para inspeccionar 'resultados'
            return resultados;
        }

        public async Task<LocalidadPostal?> ObtenerPorCodigoPostalPrimeroAsync(string codigoPostal)
        {
            if (string.IsNullOrWhiteSpace(codigoPostal))
                return null;

            return await _context.localidades_postales
                //.Include(x => x.Provincia)
                .Where(x => x.CodigoPostal.Trim() == codigoPostal.Trim())
                .FirstOrDefaultAsync();
        }
    }
}

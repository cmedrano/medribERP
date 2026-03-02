using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repository
{
    public class ArticuloRepository : IArticuloRepository
    {
        private readonly AppDbContext _context;

        public ArticuloRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Articulo>> ObtenerTodosActivosAsync()
        {
            return await _context.Articulos
                .Where(a => a.Activo)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<Articulo> ObtenerPorIdAsync(int id)
        {
            return await _context.Articulos.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Articulo> ObtenerPorCodigoAsync(string codigo)
        {
            return await _context.Articulos.FirstOrDefaultAsync(a => a.Codigo == codigo);
        }

        public async Task GuardarAsync(Articulo articulo)
        {
            articulo.CreatedAt = DateTime.UtcNow;
            articulo.UpdatedAt = DateTime.UtcNow;
            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Articulo articulo)
        {
            articulo.UpdatedAt = DateTime.UtcNow;
            _context.Articulos.Update(articulo);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo != null)
            {
                articulo.Activo = false;
                articulo.UpdatedAt = DateTime.UtcNow;
                _context.Articulos.Update(articulo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> ObtenerTotalAsync()
        {
            return await _context.Articulos.Where(a => a.Activo).CountAsync();
        }
    }
}

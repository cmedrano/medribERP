using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Ventas.ViewModels;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repository
{
    public class ArticulosPreciosRepository : IArticulosPreciosRepository
    {
        private readonly AppDbContext _context;
        public ArticulosPreciosRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PriceItemDto>> GetPreciosByArticuloAsync(int articuloId)
        {
            var precios = await _context.ArticulosPrecios
               .Where(x => x.ArticuloId == articuloId)
               .Select(x => new PriceItemDto
               {
                   ListId = x.ListaPrecioId,
                   Price = x.Precio
               })
               .ToListAsync();

            return precios;
        }
    }
}

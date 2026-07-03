using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Areas.Ventas.ViewModels;
using PresupuestoMVC.Areas.Ventas.ViewModels.DTOs;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            try
            {     
                return await _context.Articulos
                    .Where(a => a.Activo)
                    .OrderBy(a => a.Nombre)
                    .ToListAsync();
                }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Articulo> ObtenerPorIdAsync(int id)
        {
            return await _context.Articulos.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Articulo> ObtenerPorCodigoAsync(string codigo)
        {
            return await _context.Articulos.FirstOrDefaultAsync(a => a.Codigo == codigo);
        }

        public async Task GuardarAsync(Articulo articulo, List<ArticulosPrecios> articulosPrecios)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Articulos.Add(articulo);
                await _context.SaveChangesAsync();

                if (articulosPrecios != null && articulosPrecios.Any())
                {
                    foreach (var item in articulosPrecios)
                    {
                        item.ArticuloId = articulo.Id;
                    }
                    _context.ArticulosPrecios.AddRange(articulosPrecios);
                    await _context.SaveChangesAsync();
                }
                            
                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ActualizarAsync(Articulo articulo, List<ArticulosPrecios> articulosPrecios)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                articulo.UpdatedAt = DateTime.UtcNow;
                _context.Articulos.Update(articulo);
                await _context.SaveChangesAsync();

                var preciosExistentes = await _context.ArticulosPrecios
                .Where(x => x.ArticuloId == articulo.Id)
                .ToListAsync();

                if (articulosPrecios != null && articulosPrecios.Any())
                {
                    foreach (var item in articulosPrecios)
                    {
                        var existente = preciosExistentes
                         .FirstOrDefault(x => x.ListaPrecioId == item.ListaPrecioId);

                        if (existente != null)
                        {
                            if(item.Precio != 0)
                            {
                                existente.Precio = item.Precio;
                                existente.UpdatedAt = DateTime.UtcNow;
                            }
                            else
                            {
                                await _context.ArticulosPrecios
                                .Where(x => x.ArticuloId == articulo.Id && x.ListaPrecioId == item.ListaPrecioId)
                                .ExecuteDeleteAsync();
                            }
                        }
                        else if(item.Precio > 0)
                        {
                            item.ArticuloId = articulo.Id;
                            item.UpdatedAt = DateTime.UtcNow;

                            _context.ArticulosPrecios.Add(item);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
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

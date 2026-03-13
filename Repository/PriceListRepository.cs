using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using System;

namespace PresupuestoMVC.Repositories
{
    public class PriceListRepository : IPriceListRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PriceListRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PriceList>> GetAllAsync()
        {
            var priceListActivos = await _context.PriceList
                    .Where(a => a.Activo)
                    .OrderBy(a => a.Nombre)
                    .ToListAsync();
            return priceListActivos;
        }
        public async Task<PriceList?> GetByIdAsync(int id)
        {
            return await _context.PriceList.FindAsync(id);
        }

        public async Task AddListAsync(PriceList priceList)
        {
            try
            {
                _context.PriceList.Add(priceList);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAsync(UpdatePriceListViewRequest dto)
        {
            try
            {
                var entity = await _context.PriceList
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (entity == null)
                    throw new Exception("No encontrado");

                entity.Nombre = dto.Nombre;
                entity.Descripcion = dto.Descripcion;
                entity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("error al actualizar la lista");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var lista = await _context.PriceList.FindAsync(id);
            if (lista != null)
            {
                lista.Activo = false;
                lista.UpdatedAt = DateTime.UtcNow;
                _context.PriceList.Update(lista);
                await _context.SaveChangesAsync();
            }
        }
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Repository.Interfaces;

namespace PresupuestoMVC.Repository
{
    public class PeriodRepository : IPeriodRepository
    {
        private readonly AppDbContext _context;
        public PeriodRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PeriodResponseDto> CreatePeriodAsync(Periodo period)
        {
            //var accountExiste = await _context.pero.AnyAsync(r => r.nombreCuenta == account.nombreCuenta);

            //if (accountExiste)
            //    throw new InvalidOperationException("El nombre de la cuenta ya existe.");

            _context.Periodos.Add(period);
            await _context.SaveChangesAsync();
            var periodo = await _context.Periodos
                .FirstOrDefaultAsync(r => r.Fecha == period.Fecha);

            return new PeriodResponseDto
            {
                Fecha = periodo.Fecha,
                ValorPresupuestado = periodo.ValorPresupuestado
            };
        }
        public async Task UpdatePeriodAsync(Periodo periodo)
        {
            _context.Periodos.Update(periodo);
            await _context.SaveChangesAsync();
        }


    }
}

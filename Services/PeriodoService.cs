using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class PeriodoService : IPeriodoService
    {
        private readonly AppDbContext _context;
        private readonly IPeriodRepository _periodRepository;

        public PeriodoService(AppDbContext context, IPeriodRepository periodRepository)
        {
            _context = context;
            _periodRepository = periodRepository;
        }

        public async Task<List<Periodo>> GetAllPeriodosAsync()
        {
            return await _context.Periodos
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }
        public async Task<List<Year>> GetAllYearsAsync()
        {
            return await _context.Year.ToListAsync();
        }

        public async Task<List<Month>> GetAllMonthsAsync()
        {
            return await _context.Months
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int yearId, int monthId)
        {
            var year = await _context.Year.FindAsync(yearId);
            var month = await _context.Months.FindAsync(monthId);
            if (year == null || month == null) return false;

            DateTime fecha = new DateTime(year.YearValue, month.MonthNumber, 1, 0, 0, 0, DateTimeKind.Utc);
            return await _context.Periodos.AnyAsync(p => p.Fecha == fecha);
        }

        public async Task<PeriodResponseDto> CreatePeriodAsync(CreatePeriodViewRequest periodRequest)
        {
            // 1. Verificamos si ya existe (reutilizando el método ExistsAsync)
            bool existe = await ExistsAsync(periodRequest.YearId, periodRequest.MonthId);
            if (existe)
            {
                // Lanzamos esta excepción específica para que el Controller la atrape
                throw new InvalidOperationException("DUPLICADO");
            }

            var yearEntity = await _context.Year.FindAsync(periodRequest.YearId);
            var monthEntity = await _context.Months.FindAsync(periodRequest.MonthId);

            if (yearEntity == null || monthEntity == null)
                throw new Exception("Referencia de Año o Mes no encontrada.");

            DateTime fechaCalculada = new DateTime(yearEntity.YearValue, monthEntity.MonthNumber, 1, 0, 0, 0, DateTimeKind.Utc);

            var periodo = new Periodo
            {
                Fecha = fechaCalculada,
                ValorPresupuestado = periodRequest.ValorPresupuestado
            };

            return await _periodRepository.CreatePeriodAsync(periodo);
        }

        public async Task UpdatePeriodAsync(int id, CreatePeriodViewRequest periodRequest)
        {
            var periodoExistente = await _context.Periodos.FindAsync(id);
            if (periodoExistente == null) throw new Exception("El periodo solicitado no existe.");

            var yearEntity = await _context.Year.FindAsync(periodRequest.YearId);
            var monthEntity = await _context.Months.FindAsync(periodRequest.MonthId);

            if (yearEntity == null || monthEntity == null)
                throw new Exception("Referencia de Año o Mes no válida.");

            DateTime nuevaFecha = new DateTime(yearEntity.YearValue, monthEntity.MonthNumber, 1, 0, 0, 0, DateTimeKind.Utc);

            var esDuplicado = await _context.Periodos.AnyAsync(p => p.Fecha == nuevaFecha && p.Id != id);
            if (esDuplicado)
            {
                throw new InvalidOperationException("DUPLICADO");
            }

            periodoExistente.Fecha = nuevaFecha;
            periodoExistente.ValorPresupuestado = periodRequest.ValorPresupuestado;

            await _periodRepository.UpdatePeriodAsync(periodoExistente);
        }
    }
}

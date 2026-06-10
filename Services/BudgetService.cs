using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PresupuestoMVC.Data;
using PresupuestoMVC.Migrations;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Services.Interfaces;
using System.ComponentModel.Design;

namespace PresupuestoMVC.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public BudgetService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BudgetResponseDTO> GetByIdAsync(int id)
        {
            var rubro = await _context.Budget
                .Include(r => r.tipoRubro)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (rubro == null)
                throw new Exception($"Rubro con ID {id} no encontrado.");

            return _mapper.Map<BudgetResponseDTO>(rubro);
        }

        public async Task<IEnumerable<BudgetResponseDTO>> GetAllBudgetAsync()
        {
            var rubros = await _context.Budget.ToListAsync();
            return _mapper.Map<IEnumerable<BudgetResponseDTO>>(rubros);
        }

        public async Task<IEnumerable<RubroType>> GetAllRubroTypesAsync()
        {
            var rubroTypes = await _context.RubroType
                .Where(rt => rt.RubroPadreId == null)
                .OrderBy(rt => rt.Id)
                .ToListAsync();
            return rubroTypes;
        }

        public async Task<BudgetResponseDTO> CreateAsync(CreateBudgetViewRequest createDto)
        {
            try
            {
                // Validaciones
                var tipoExiste = await _context.RubroType.AnyAsync(rt => rt.Id == createDto.rubroTypeId);
                var subRubroExiste = await _context.RubroType.AnyAsync(rt => rt.Id == createDto.SubRubroId);

                var budgetExiste = await _context.Budget.Where(
                    r => r.Mes == createDto.Mes &&
                    r.Anio == createDto.Anio &&
                    r.RubroTypeId == createDto.rubroTypeId)
                    .FirstOrDefaultAsync();

                var budgetWithSubRubro = await _context.Budget.Where(
                    r => r.Mes == createDto.Mes &&
                    r.Anio == createDto.Anio &&
                    r.RubroTypeId == createDto.SubRubroId)
                    .FirstOrDefaultAsync();

                if (!tipoExiste)
                    throw new Exception($"Tipo de rubro con ID {createDto.rubroTypeId} no existe.");

                if (createDto.valorInicial < 0)
                    throw new Exception("El valor inicial no puede ser negativo.");

                if (createDto.Mes < 1 || createDto.Mes > 12)
                    throw new Exception("El mes debe estar entre 1 y 12.");

                createDto.CreateDate = DateTime.UtcNow;

                if (budgetExiste == null)
                {
                    var budget = new Budget()
                    {
                        RubroTypeId = createDto.rubroTypeId,
                        valorInicial = createDto.valorInicial,
                        Mes = createDto.Mes,
                        Anio = createDto.Anio,
                        CreateByUserId = createDto.CreateByUserId,
                        CompanyId = createDto.CompanyId,
                        CreateDate = DateTime.UtcNow
                    };
                    _context.Budget.Add(budget);
                }

                if (budgetWithSubRubro == null && createDto.SubRubroId != null)
                {
                    var budgetWithSubCategory = new Budget()
                    {
                        RubroTypeId = createDto.SubRubroId,
                        valorInicial = createDto.valorInicial,
                        Mes = createDto.Mes,
                        Anio = createDto.Anio,
                        CreateByUserId = createDto.CreateByUserId,
                        CompanyId = createDto.CompanyId,
                        CreateDate = DateTime.UtcNow
                    };
                    _context.Budget.Add(budgetWithSubCategory);

                    if (budgetExiste != null)
                        budgetExiste.valorInicial += budgetWithSubCategory.valorInicial;   
                }

                await _context.SaveChangesAsync();

                var result = await _context.Budget
                    .Include(r => r.tipoRubro)
                    .FirstOrDefaultAsync(r => r.Id == 4);

                return _mapper.Map<BudgetResponseDTO>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BudgetResponseDTO> UpdateAsync(int id, UpdateBudgetViewRequest updateDto)
        {
            try
            {
                // Validar existencia
                var existingRubro = await _context.Budget
                    .Include(r => r.tipoRubro)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (existingRubro == null)
                    throw new Exception($"Rubro con ID {id} no encontrado.");

                // Validaciones
                var tipoExiste = await _context.RubroType.AnyAsync(rt => rt.Id == updateDto.RubroTypeId);

                if (!tipoExiste)
                    throw new Exception($"Tipo de rubro con ID {updateDto.RubroTypeId} no existe.");

                if (updateDto.valorInicial < 0)
                    throw new Exception("El valor inicial no puede ser negativo.");

                if (updateDto.Mes < 1 || updateDto.Mes > 12)
                    throw new Exception("El mes debe estar entre 1 y 12.");

                // Actualizar

                existingRubro.CreateDate = DateTime.SpecifyKind(
                    existingRubro.CreateDate,
                     DateTimeKind.Utc
                );

                existingRubro.RubroTypeId = updateDto.RubroTypeId;
                existingRubro.valorInicial = updateDto.valorInicial;
                existingRubro.Mes = updateDto.Mes;
                existingRubro.Anio = updateDto.Anio;
                existingRubro.UpdateDate = DateTime.UtcNow;

                var result = _context.Budget.Update(existingRubro);
                await _context.SaveChangesAsync();

                var response = new BudgetResponseDTO()
                {
                    Id = existingRubro.Id,
                    valorInicial = existingRubro.valorInicial,
                    valorGastado = existingRubro.ValorGastado,
                    Mes = existingRubro.Mes,
                    Anio = existingRubro.Anio
                };
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existsRubro = await _context.Budget.AnyAsync(g => g.Id == id);

            if (!existsRubro)
                throw new Exception($"Rubro con ID {id} no encontrado.");

            var rubro = await _context.Budget.FindAsync(id);
            if (rubro == null)
                return false;

            _context.Budget.Remove(rubro);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RubroType>> GetTiposRubroAsync()
        {
            var tipos = await _context.RubroType
                .OrderBy(rt => rt.nombreRubro)
                .ToListAsync();

            return _mapper.Map<List<RubroType>>(tipos);
        }

        public async Task<PaginacionRespuestaDto<BudgetGroupedDto>> GetFiltradosAsync(FiltroBudgetViewRequest filtro, int pagina, int tamañoPagina, int companyId)
        {
            try
            {
                // Validar parámetros de paginación
                if (filtro.Pagina < 1)
                    throw new Exception("La página debe ser mayor a 0.");

                if (filtro.TamañoPagina < 1 || filtro.TamañoPagina > 100)
                    throw new Exception("El tamaño de página debe estar entre 1 y 100.");

                // Validar que el RubroTypeId existe
                if (filtro.RubroTypeId.HasValue && filtro.RubroTypeId.Value > 0)
                {
                    var tipoExiste = await _context.RubroType.AnyAsync(rt => rt.Id == filtro.RubroTypeId.Value);
                    if (!tipoExiste)
                        throw new Exception($"Tipo de rubro con ID {filtro.RubroTypeId} no existe.");
                }

                // Obtener datos filtrados y paginados
                var query = _context.Budget
                    .Where(r => r.CompanyId == companyId)
                    .Include(r => r.tipoRubro)
                    .AsQueryable();

                var padresQuery = _context.Budget
                    .Where(b => b.CompanyId == companyId &&
                                b.tipoRubro.RubroPadreId == null)
                    .Include(b => b.tipoRubro)
                    .AsQueryable();

                //var cont = await query.CountAsync();

                // Aplicar filtros
                if (filtro.Mes.HasValue && filtro.Mes.Value > 0)
                {
                    padresQuery = padresQuery.Where(r => r.Mes == filtro.Mes.Value);
                }

                if (filtro.Anio.HasValue && filtro.Anio.Value > 0)
                {
                    padresQuery = padresQuery.Where(r => r.Anio == filtro.Anio.Value);
                }

                if (filtro.RubroTypeId.HasValue && filtro.RubroTypeId.Value > 0)
                {
                    padresQuery = padresQuery.Where(r => r.RubroTypeId == filtro.RubroTypeId.Value);
                }

                if (filtro.Deficit)
                {
                    padresQuery = padresQuery.Where(x => x.ValorGastado > x.valorInicial);
                }

                // Aplicar paginación
                var rubros = await padresQuery
                    .OrderBy(r => r.Id)
                    .ThenBy(r => r.Anio)
                    .ThenBy(r => r.Mes)
                    .ThenBy(r => r.tipoRubro.nombreRubro)
                    .Skip((pagina - 1) * tamañoPagina)
                    .Take(tamañoPagina)
                    .ToListAsync();

                var parentIds = rubros.Select(p => p.RubroTypeId).ToList();

                var subRubrosQuery = _context.Budget
                    .Where(b => b.CompanyId == companyId &&
                     b.tipoRubro.RubroPadreId != null &&
                     parentIds.Contains(b.tipoRubro.RubroPadreId.Value));

                if (filtro.Mes.HasValue && filtro.Mes > 0)
                    subRubrosQuery = subRubrosQuery.Where(x => x.Mes == filtro.Mes);

                if (filtro.Anio.HasValue && filtro.Anio > 0)
                    subRubrosQuery = subRubrosQuery.Where(x => x.Anio == filtro.Anio);

                if (filtro.Deficit)
                    subRubrosQuery = subRubrosQuery.Where(x => x.ValorGastado > x.valorInicial);

                //var subRubros = await _context.Budget
                //    .Where(b => b.CompanyId == companyId &&
                //                b.tipoRubro.RubroPadreId != null &&
                //                parentIds.Contains(b.tipoRubro.RubroPadreId.Value))
                //    .Include(b => b.tipoRubro)
                //    .ToListAsync();

                var subRubros = await subRubrosQuery
                    .Include(b => b.tipoRubro)
                    .ToListAsync();

                var grouped = rubros.Select(p => new BudgetGroupedDto
                {
                    Budget = p,
                    SubBudget = subRubros
                        .Where(s => s.tipoRubro.RubroPadreId == p.RubroTypeId)
                        .ToList()
                }).ToList();

                // Obtener total de registros
                var totalRegistros = await query.CountAsync();

                var respuesta = new PaginacionRespuestaDto<BudgetGroupedDto>
                {
                    Datos = grouped,
                    PaginaActual = filtro.Pagina,
                    TamañoPagina = filtro.TamañoPagina,
                    TotalRegistros = totalRegistros
                };

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesbyDateAsync(DateTime date)
        {
            int month = date.Month;
            int year = date.Year;
            var categories = await _context.Budget
                .Include(b => b.tipoRubro)
                .Where(b => b.Mes == month && b.Anio == year)
                .Select(b => new CategoryResponseDto
                {
                   Id = b.tipoRubro.Id,
                   nombreRubro = b.tipoRubro.nombreRubro
                })
                .Distinct()
                .ToListAsync();

            return categories;
        }
        public async Task<int> GetBudgetCountAsync(int companyId)
        {
            var totalBudget = await _context.Budget
                .Where(b => b.CompanyId == companyId)
                .CountAsync();
            return totalBudget;
        }
    }
}

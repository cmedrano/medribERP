using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using System.Net;

namespace PresupuestoMVC.Repository
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public ProviderRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IEnumerable<ProviderResponseDto>> GetAllProviderAsync(int compnayId)
        {
            try
            {
                var providers = await _context.Provider
                    .Where(p => p.CompanyId == compnayId)
                    .ToListAsync();

                var providerDto = providers.Select(x => new ProviderResponseDto()
                {
                    Id = x.Id,
                    Company = x.Company,
                    Code = x.Code
                }).ToList();
                return providerDto;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<PaginatedResult<ProviderResponseDto>> GetPagedAsync(int pageNumber, int pageSize, int companyId)
        {
            var query = _context.Provider
                .Where(a => a.CompanyId == companyId)
                .OrderBy(a => a.Company);

            var queryProvider = query.Select(p => new ProviderResponseDto
            {
                Id = p.Id,
                Code = p.Code,
                Company = p.Company,
                Phone = p.Phone,
                Address = p.Address,
                Email = p.Email,
                Responsible = p.Responsible
            });

            var totalCount = await queryProvider.CountAsync();
            var items = await queryProvider
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<ProviderResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ProviderResponseDto> CreateSupplierAsync(CreateSupplierViewRequest supplierDto)
        {
            try
            {
                var providerExiste = await _context.Provider
                .AnyAsync(r => r.Company == supplierDto.Empresa && r.CompanyId == supplierDto.CompanyId);

                if (providerExiste)
                    throw new InvalidOperationException("El nombre del proveedor ya existe.");

                var proveedor = new Provider()
                {
                    Code = supplierDto.Codigo,
                    Company = supplierDto.Empresa,
                    CompanyId = supplierDto.CompanyId,
                    Phone = supplierDto.Telefono,
                    Address = supplierDto.Direccion,
                    Email = supplierDto.Mail,
                    Responsible = supplierDto.Responsable
                };

                _context.Provider.Add(proveedor);
                await _context.SaveChangesAsync();
                var createdSupplier = await _context.Provider
                    .FirstOrDefaultAsync(r => r.Company == supplierDto.Empresa);

                return new ProviderResponseDto
                {
                    Id = createdSupplier.Id,
                    Company = createdSupplier.Company,
                    Code = createdSupplier.Code
                };
            }
            catch
            {
                throw new InvalidOperationException("error al crear el proveedor");
            }
        }
        public async Task<ProviderResponseDto> UpdateSupplierAsync(UpdateSupplierViewRequest supplierDto)
        {
            try
            {
                var providerExiste = await _context.Provider
                .FirstOrDefaultAsync(r => r.Id == supplierDto.Id);

                if (providerExiste == null)
                    throw new InvalidOperationException("El proveedor no existe.");

                providerExiste.Company = supplierDto.Empresa;
                providerExiste.Code = supplierDto.Codigo;
                providerExiste.Phone = supplierDto.Telefono;
                providerExiste.Address = supplierDto.Direccion;
                providerExiste.Email = supplierDto.Mail;
                providerExiste.Responsible = supplierDto.Responsable;

                await _context.SaveChangesAsync();

                var responseDto = new ProviderResponseDto()
                {
                    Id = providerExiste.Id,
                    Company = providerExiste.Company,
                    Code = providerExiste.Code,
                };
                return responseDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("no se pudo actualizar el proveedor");
            }
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            try
            {
                var providerExiste = await _context.Provider
                .FirstOrDefaultAsync(r => r.Id == id);

                if (providerExiste == null)
                    throw new InvalidOperationException("El proveedor no existe.");

                _context.Provider.Remove(providerExiste);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

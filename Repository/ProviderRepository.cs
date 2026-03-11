using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PresupuestoMVC.Data;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Repository.Interfaces;

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
        public async Task<IEnumerable<ProviderResponseDto>> GetAllProviderAsync()
        {
            try
            {
                var providers = await _context.Provider.ToListAsync();

                var providerDto = providers.Select(x => new ProviderResponseDto()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return providerDto;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}

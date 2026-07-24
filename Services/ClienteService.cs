using AutoMapper;
using PresupuestoMVC.Areas.Accounting.Data.DTOs;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper, IActivityLogRepository activityLogRepository)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _activityLogRepository = activityLogRepository;
        }

        public async Task<IEnumerable<ClienteResponseDTO>> ObtenerTodosAsync(int companyId)
        {
            var clientes = await _clienteRepository.ObtenerTodosAsync(companyId);
            return _mapper.Map<IEnumerable<ClienteResponseDTO>>(clientes);
        }

        public async Task<ClienteResponseDTO?> ObtenerPorIdAsync(int id)
        {
            var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
            return _mapper.Map<ClienteResponseDTO?>(cliente);
        }

        public async Task<ClienteResponseDTO?> ObtenerPorCuitAsync(string cuit)
        {
            var cliente = await _clienteRepository.ObtenerPorCuitAsync(cuit);
            return _mapper.Map<ClienteResponseDTO?>(cliente);
        }

        public async Task<ClienteResponseDTO?> ObtenerPorEmailAsync(string email)
        {
            var cliente = await _clienteRepository.ObtenerPorEmailAsync(email);
            return _mapper.Map<ClienteResponseDTO?>(cliente);
        }

        public async Task<ClienteResponseDTO> GuardarAsync(CreateClienteViewRequest createDto)
        {
            try
            {
                if (createDto == null)
                    throw new Exception($"El cliente no puede ser nulo: {nameof(createDto)}");

                if (string.IsNullOrWhiteSpace(createDto.Nombre))
                    throw new Exception("El nombre del cliente es obligatorio");

                var cliente = _mapper.Map<Cliente>(createDto);
                await _clienteRepository.GuardarAsync(cliente);

                var ActivityDto = new ActivityLogRequestDto()
                {
                    CompanyId = createDto.CompanyId,
                    EntityType = "Cliente",
                    Action = "CREATE",
                    Description = $"Se creó un nuevo cliente {cliente.Nombre}"
                };
                await _activityLogRepository.LogAsync(ActivityDto);

                return _mapper.Map<ClienteResponseDTO>(cliente);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ClienteResponseDTO> ActualizarAsync(UpdateClienteViewRequest updateDto)
        {
            if (updateDto == null)
                throw new Exception($"El cliente no puede ser nulo: {nameof(updateDto)}");

            var cliente = await _clienteRepository.ObtenerPorIdAsync(updateDto.Id);
            if (cliente == null)
                throw new Exception("Cliente no encontrado");

            _mapper.Map(updateDto, cliente);
            await _clienteRepository.ActualizarAsync(cliente);

            return _mapper.Map<ClienteResponseDTO>(cliente);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
            if (cliente == null)
                throw new Exception("Cliente no encontrado");

            await _clienteRepository.EliminarAsync(id);
            return true;
        }

        public async Task<int> ObtenerTotalAsync(int companyId)
        {
            return await _clienteRepository.ObtenerTotalAsync(companyId);
        }

        public async Task<int> GetActiveClientsCountAsync(int companyId)
        {
            return await _clienteRepository.ObtenerCantidadDeClientesActivos(companyId);
        }

        public async Task<PaginacionRespuestaDto<ClienteResponseDTO>> ObtenerPaginadosAsync(FiltroClienteViewRequest filtro, int pageNumber, int pageSize, int companyId)
        {
            if (pageNumber < 1)
                throw new Exception("La página debe ser mayor a 0.");

            if (pageSize < 1 || pageSize > 100)
                throw new Exception("El tamaño de página debe estar entre 1 y 100.");

            var resultado = await _clienteRepository.ObtenerPaginadosAsync(
                pageNumber, 
                pageSize,
                companyId,
                filtro.SearchNombre, 
                filtro.SearchFantasia
            );

            var datosDto = _mapper.Map<List<ClienteResponseDTO>>(resultado.Datos);

            return new PaginacionRespuestaDto<ClienteResponseDTO>
            {
                Datos = datosDto,
                PaginaActual = resultado.PaginaActual,
                TamañoPagina = resultado.TamañoPagina,
                TotalRegistros = resultado.TotalRegistros
            };
        }
    }
}

using AutoMapper;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClienteResponseDTO>> ObtenerTodosAsync()
        {
            var clientes = await _clienteRepository.ObtenerTodosAsync();
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
            if (createDto == null)
                throw new Exception($"El cliente no puede ser nulo: {nameof(createDto)}");

            if (string.IsNullOrWhiteSpace(createDto.Nombre))
                throw new Exception("El nombre del cliente es obligatorio");

            var cliente = _mapper.Map<Cliente>(createDto);
            await _clienteRepository.GuardarAsync(cliente);
            
            return _mapper.Map<ClienteResponseDTO>(cliente);
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

        public async Task<int> ObtenerTotalAsync()
        {
            return await _clienteRepository.ObtenerTotalAsync();
        }

        public async Task<PaginacionRespuestaDto<ClienteResponseDTO>> ObtenerPaginadosAsync(FiltroClienteViewRequest filtro, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new Exception("La página debe ser mayor a 0.");

            if (pageSize < 1 || pageSize > 100)
                throw new Exception("El tamaño de página debe estar entre 1 y 100.");

            var resultado = await _clienteRepository.ObtenerPaginadosAsync(
                pageNumber, 
                pageSize, 
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

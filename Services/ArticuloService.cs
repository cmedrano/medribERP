using AutoMapper;
using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using PresupuestoMVC.Models.ViewModels;
using PresupuestoMVC.Repository.Interfaces;
using PresupuestoMVC.Services.Interfaces;

namespace PresupuestoMVC.Services
{
    public class ArticuloService : IArticuloService
    {
        private readonly IArticuloRepository _articuloRepository;
        private readonly IMapper _mapper;

        public ArticuloService(IArticuloRepository articuloRepository, IMapper mapper)
        {
            _articuloRepository = articuloRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticuloResponseDTO>> ObtenerTodosActivosAsync()
        {
            var articulos = await _articuloRepository.ObtenerTodosActivosAsync();
            return _mapper.Map<IEnumerable<ArticuloResponseDTO>>(articulos);
        }

        public async Task<ArticuloResponseDTO> ObtenerPorIdAsync(int id)
        {
            var articulo = await _articuloRepository.ObtenerPorIdAsync(id);
            return _mapper.Map<ArticuloResponseDTO>(articulo);
        }

        public async Task<int> ObtenerTotalAsync()
        {
            return await _articuloRepository.ObtenerTotalAsync();
        }

        public async Task<ArticuloResponseDTO> CrearAsync(ArticuloCreateDTO createDto)
        {
            throw new Exception("Prueba de error");
            if (createDto == null)
                throw new Exception("Los datos del artículo no pueden ser nulos");

            if (string.IsNullOrWhiteSpace(createDto.Codigo))
                throw new Exception("El código del artículo es obligatorio");

            if (string.IsNullOrWhiteSpace(createDto.Nombre))
                throw new Exception("El nombre del artículo es obligatorio");

            var articuloExistente = await _articuloRepository.ObtenerPorCodigoAsync(createDto.Codigo);
            if (articuloExistente != null && articuloExistente.Activo)
                throw new Exception("Ya existe un artículo con este código");

            var articulo = _mapper.Map<Articulo>(createDto);
            articulo.Activo = true;

            await _articuloRepository.GuardarAsync(articulo);

            return _mapper.Map<ArticuloResponseDTO>(articulo);
        }

        public async Task<ArticuloResponseDTO> ActualizarAsync(ArticuloUpdateDTO updateDto)
        {
            if (updateDto == null)
                throw new Exception("Los datos del artículo no pueden ser nulos");

            if (updateDto.Id <= 0)
                throw new Exception("El ID del artículo es inválido");

            if (string.IsNullOrWhiteSpace(updateDto.Codigo))
                throw new Exception("El código del artículo es obligatorio");

            if (string.IsNullOrWhiteSpace(updateDto.Nombre))
                throw new Exception("El nombre del artículo es obligatorio");

            var articulo = await _articuloRepository.ObtenerPorIdAsync(updateDto.Id);
            if (articulo == null)
                throw new Exception("El artículo no existe");

            var articuloConCodigo = await _articuloRepository.ObtenerPorCodigoAsync(updateDto.Codigo);
            if (articuloConCodigo != null && articuloConCodigo.Id != updateDto.Id && articuloConCodigo.Activo)
                throw new Exception("Ya existe otro artículo con este código");

            _mapper.Map(updateDto, articulo);
            articulo.UpdatedAt = DateTime.UtcNow;

            await _articuloRepository.ActualizarAsync(articulo);

            return _mapper.Map<ArticuloResponseDTO>(articulo);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            if (id <= 0)
                throw new Exception("El ID del artículo es inválido");

            var articulo = await _articuloRepository.ObtenerPorIdAsync(id);
            if (articulo == null)
                throw new Exception("El artículo no existe");

            await _articuloRepository.EliminarAsync(id);

            return true;
        }
    }
}

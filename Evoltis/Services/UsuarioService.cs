using AutoMapper;
using Evoltis.Dto;
using Evoltis.Models;
using Evoltis.Repositories;

namespace Evoltis.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;           
        }

        public Task CrearUsuarioAsync(Usuario usuario)
        {
            return _usuarioRepository.CrearUsuarioAsync(usuario);
        }
        public Task<List<Usuario>> BuscarUsuariosAsync(FiltrosUsuarioDTO filtros)
        {
            return _usuarioRepository.BuscarUsuariosAsync(filtros);
        }
        public Task<Usuario?> BuscarUsuarioPorIdAsync(int id)
        {
            return _usuarioRepository.BuscarUsuarioPorIdAsync(id);
        }
        public async Task<Usuario?> EditarUsuarioAsync(Usuario usuario, EditarUsuarioDTO dto)
        {
            _mapper.Map(dto, usuario);

            if (dto.Domicilio != null)
            {
                if (usuario.Domicilio == null)
                    usuario.Domicilio = _mapper.Map<Domicilio>(dto.Domicilio);
                else
                    _mapper.Map(dto.Domicilio, usuario.Domicilio);
            }

            await _usuarioRepository.EditarUsuarioAsync(usuario);
            return usuario;
        }
        public Task EliminarUsuarioAsync(int id)
        {
            return _usuarioRepository.EliminarUsuarioAsync(id);
        }
    }
}

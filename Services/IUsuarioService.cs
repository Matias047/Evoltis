using Evoltis.Dto;
using Evoltis.Models;

namespace Evoltis.Services
{
    public interface IUsuarioService
    {
        Task CrearUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> BuscarUsuariosAsync(FiltrosUsuarioDTO filtros);
        Task<Usuario?> BuscarUsuarioPorIdAsync(int id);
        Task<Usuario?> EditarUsuarioAsync(Usuario usuario, EditarUsuarioDTO dto);
        Task EliminarUsuarioAsync(int id);
    }
}

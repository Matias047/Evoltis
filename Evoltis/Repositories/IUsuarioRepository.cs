using Evoltis.Dto;
using Evoltis.Models;

namespace Evoltis.Repositories
{
    public interface IUsuarioRepository
    {
        Task CrearUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> BuscarUsuariosAsync(FiltrosUsuarioDTO filtros);
        Task<Usuario?> BuscarUsuarioPorIdAsync(int id);
        Task EditarUsuarioAsync(Usuario usuario);
        Task<bool> EliminarUsuarioAsync(int id);
    }
}

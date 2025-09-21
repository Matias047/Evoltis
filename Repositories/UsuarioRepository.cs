using Evoltis.Data;
using Evoltis.Dto;
using Evoltis.Models;
using Microsoft.EntityFrameworkCore;

namespace Evoltis.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CrearUsuarioAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Usuario>> BuscarUsuariosAsync(FiltrosUsuarioDTO filtros)
        {
            var query = _context.Usuarios
            .Include(u => u.Domicilio)
            .AsQueryable();

            if (!string.IsNullOrEmpty(filtros.Nombre))
                query = query.Where(u => u.Nombre.Contains(filtros.Nombre));

            if (!string.IsNullOrEmpty(filtros.Provincia))
                query = query.Where(u => u.Domicilio != null &&
                                         u.Domicilio.Provincia.Contains(filtros.Provincia));

            if (!string.IsNullOrEmpty(filtros.Ciudad))
                query = query.Where(u => u.Domicilio != null &&
                                         u.Domicilio.Ciudad.Contains(filtros.Ciudad));

            return await query.ToListAsync();

        }
        public async Task<Usuario?> BuscarUsuarioPorIdAsync(int id)
        {
            return await _context.Usuarios
                                 .Include(u => u.Domicilio)
                                 .FirstOrDefaultAsync(u => u.ID == id);
        }
        public async Task EditarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}

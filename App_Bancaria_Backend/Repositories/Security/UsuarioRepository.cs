using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace App_Bancaria_Backend.Repositories.Security
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuario
                .FirstOrDefaultAsync(x => x.Email == email && !x.FlgEli);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Usuario.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ExisteTelefonoAsync(string telefono)
        {
            return await _context.Usuario.AnyAsync(x => x.NroTelefono == telefono);
        }

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }
    }
}

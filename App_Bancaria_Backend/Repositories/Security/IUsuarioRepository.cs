using App_Bancaria_Backend.Models;

namespace App_Bancaria_Backend.Repositories.Security
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByEmailAsync(string email);

        Task<bool> ExisteEmailAsync(string email);
        Task<bool> ExisteTelefonoAsync(string telefono);
        Task AddAsync(Usuario usuario);
    }
}

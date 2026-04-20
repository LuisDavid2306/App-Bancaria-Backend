using App_Bancaria_Backend.DTOs.Admin;
using App_Bancaria_Backend.Models;

namespace App_Bancaria_Backend.Repositories.Admin
{
    public interface IAdminRepository
    {
        Task<List<UsuarioAdminDto>> GetUsuariosAsync();
        Task<List<TransaccionAdminDto>> GetTransaccionesAsync();
        Task<List<GrupoAdminDto>> GetGruposAsync();
        Task<DashboardDto> GetDashboardAsync();
        Task AddUsuarioAsync(Usuario usuario);
        Task<Usuario> GetUsuarioById(int id);
        Task<GrupoAhorro> GetGrupoByIdAsync(int id);
        Task DeleteGrupoAsync(GrupoAhorro grupo);
        Task DeleteUsuarioAsync(Usuario usuario);
        Task SaveChangesAsync();
    }
}

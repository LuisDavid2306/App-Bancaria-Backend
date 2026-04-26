using App_Bancaria_Backend.DTOs.Admin;
using App_Bancaria_Backend.DTOs.Base;
using App_Bancaria_Backend.Models;

namespace App_Bancaria_Backend.Repositories.Admin
{
    public interface IAdminRepository
    {
        Task<List<UsuarioAdminDto>> GetUsuariosAsync(string? search);
        Task<List<TransaccionAdminDto>> GetTransaccionesAsync(DateTime? inicio, DateTime? fin, string? tipo, string? usuario);
        Task<List<GrupoAdminDto>> GetGruposAsync();
        Task<DashboardDto> GetDashboardAsync(DateTime? fechaInicio, DateTime? fechaFin);
        Task AddUsuarioAsync(Usuario usuario);
        Task<Usuario> GetUsuarioById(int id);
        Task<GrupoAhorro> GetGrupoByIdAsync(int id);
        Task DeleteGrupoAsync(GrupoAhorro grupo);
        Task DeleteUsuarioAsync(Usuario usuario);
        Task<GrupoDetalleDto> GetGrupoDetalleAsync(string codgrupo);
        Task SaveChangesAsync();
    }
}

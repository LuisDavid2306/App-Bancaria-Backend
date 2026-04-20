using App_Bancaria_Backend.Models;

namespace App_Bancaria_Backend.Repositories.GrupoAhorros
{
    public interface IGrupoAhorroRepository
    {
        Task AddGrupoAsync(GrupoAhorro grupo);
        Task AddMiembroAsync(GrupoMiembro miembro);
        Task AddAdministradorAsync(GrupoAdministrador admin);
        Task<bool> ExisteMiembro(int idUsuario, int idGrupo);
        Task<GrupoAhorro> GetGrupoById(int idGrupo);
        Task<Usuario> GetUsuarioConCuenta(int idUsuario);
        Task<bool> EsMiembro(int idUsuario, int idGrupo);
        Task<TipoTransaccion> GetTipoTransaccionPorNombre(string nombre);
        Task<Retiro> GetRetiroById(int idRetiro);
        Task<bool> YaAprobo(int idUsuario, int idRetiro);
        Task<int> CantidadAdmins(int idGrupo);
        Task<int> CantidadAprobaciones(int idRetiro);
        Task AddAprobacionAsync(RetiroAprobacion aprobacion);
        Task AddRetiroAsync(Retiro retiro);
        Task AddAporteAsync(Aporte aporte);
        Task SaveChangesAsync();
    }
}

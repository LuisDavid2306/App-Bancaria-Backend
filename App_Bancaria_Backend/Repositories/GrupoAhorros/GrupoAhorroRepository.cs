using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Bancaria_Backend.Repositories.GrupoAhorros
{
    public class GrupoAhorroRepository : IGrupoAhorroRepository
    {
        private readonly AppDbContext _context;
        public GrupoAhorroRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddGrupoAsync(GrupoAhorro grupo)
        {
            await _context.GrupoAhorro.AddAsync(grupo);
        }

        public async Task AddMiembroAsync(GrupoMiembro miembro)
        {
            await _context.GrupoMiembro.AddAsync(miembro);
        }

        public async Task AddAdministradorAsync(GrupoAdministrador admin)
        {
            await _context.GrupoAdministrador.AddAsync(admin);
        }
        public async Task<bool> ExisteMiembro(int idUsuario, int idGrupo)
        {
            return await _context.GrupoMiembro
                .AnyAsync(x => x.IdUsuario == idUsuario && x.IdGrupo == idGrupo);
        }
        public async Task<GrupoAhorro> GetGrupoById(int idGrupo)
        {
            return await _context.GrupoAhorro
                .FirstOrDefaultAsync(x => x.IdGrupo == idGrupo);
        }
        public async Task<Usuario> GetUsuarioConCuenta(int idUsuario)
        {
            return await _context.Usuario
                .Include(u => u.Cuenta)
                .FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);
        }

        public async Task<bool> EsMiembro(int idUsuario, int idGrupo)
        {
            return await _context.GrupoMiembro
                .AnyAsync(x => x.IdUsuario == idUsuario && x.IdGrupo == idGrupo);
        }

        public async Task<TipoTransaccion> GetTipoTransaccionPorNombre(string nombre)
        {
            return await _context.TipoTransaccion
                .FirstOrDefaultAsync(x => x.Nombre == nombre);
        }

        public async Task<Retiro> GetRetiroById(int idRetiro)
        {
            return await _context.Retiro.FirstOrDefaultAsync(x => x.IdRetiro == idRetiro);
        }

        public async Task<bool> YaAprobo(int idUsuario, int idRetiro)
        {
            return await _context.RetiroAprobacion
                .AnyAsync(x => x.IdUsuario == idUsuario && x.IdRetiro == idRetiro);
        }

        public async Task<int> CantidadAdmins(int idGrupo)
        {
            return await _context.GrupoAdministrador
                .CountAsync(x => x.IdGrupo == idGrupo);
        }

        public async Task<int> CantidadAprobaciones(int idRetiro)
        {
            return await _context.RetiroAprobacion
                .CountAsync(x => x.IdRetiro == idRetiro && x.Aprobado);
        }

        public async Task AddAprobacionAsync(RetiroAprobacion aprobacion)
        {
            await _context.RetiroAprobacion.AddAsync(aprobacion);
        }

        public async Task AddRetiroAsync(Retiro retiro)
        {
            await _context.Retiro.AddAsync(retiro);
        }
        public async Task AddAporteAsync(Aporte aporte)
        {
            await _context.Aporte.AddAsync(aporte);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

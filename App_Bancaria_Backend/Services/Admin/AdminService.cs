using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.DTOs.Admin;
using App_Bancaria_Backend.DTOs.Base;
using App_Bancaria_Backend.Models;
using App_Bancaria_Backend.Repositories.Admin;
using Microsoft.EntityFrameworkCore;

namespace App_Bancaria_Backend.Services.Admin
{
    public class AdminService
    {
        private readonly IAdminRepository _repo;
        private readonly AppDbContext _context;

        public AdminService(IAdminRepository repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<ApiResponse<List<UsuarioAdminDto>>> ObtenerUsuariosAsync(string? search)
        {
            var data = await _repo.GetUsuariosAsync(search);

            return new ApiResponse<List<UsuarioAdminDto>>(true, "OK", data);
        }

        public async Task<ApiResponse<List<TransaccionAdminDto>>> ObtenerTransaccionesAsync(DateTime? inicio, DateTime? fin, string? tipo, string? usuario)
        {
            var data = await _repo.GetTransaccionesAsync(inicio, fin, tipo, usuario);
            return new ApiResponse<List<TransaccionAdminDto>>(true, "OK", data);
        }
        public async Task<ApiResponse<List<GrupoAdminDto>>> ObtenerGruposAsync()
        {
            var data = await _repo.GetGruposAsync();

            return new ApiResponse<List<GrupoAdminDto>>(true, "OK", data);
        }
        public async Task<ApiResponse<DashboardDto>> ObtenerDashboardAsync(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var data = await _repo.GetDashboardAsync(fechaInicio, fechaFin);
            return new ApiResponse<DashboardDto>(true, "OK", data);
        }
        public async Task<ApiResponse<string>> CrearUsuarioAsync(CrearUsuarioAdminDto dto)
        {
            var usuario = new Usuario
            {
                CodUsuario = Guid.NewGuid().ToString(),
                Nombre = dto.Nombre,
                ApePat = dto.ApePat,
                ApeMate = dto.ApeMate,
                Email = dto.Email,
                NroTelefono = dto.NroTelefono,
                PasswordHash = dto.Password,
                FlgEstado = true,
                FlgEli = false
            };

            await _repo.AddUsuarioAsync(usuario);
            await _repo.SaveChangesAsync();

            var cuenta = new Cuenta
            {
                CodCuenta = Guid.NewGuid().ToString(),
                IdUsuario = usuario.IdUsuario,
                Saldo = 0,
                CodQR = Guid.NewGuid().ToString()
            };

            _context.Cuenta.Add(cuenta);

            await _repo.SaveChangesAsync();

            return new ApiResponse<string>(true, "Usuario creado", "OK");
        }
        public async Task<ApiResponse<string>> EditarUsuarioAsync(EditarUsuarioAdminDto dto)
        {
            var usuario = await _repo.GetUsuarioById(dto.IdUsuario);

            if (usuario == null)
                return new ApiResponse<string>(false, "Usuario no existe", null);

            usuario.Nombre = dto.Nombre;
            usuario.ApePat = dto.ApePat;
            usuario.ApeMate = dto.ApeMate;
            usuario.NroTelefono = dto.NroTelefono;

            await _repo.SaveChangesAsync();

            return new ApiResponse<string>(true, "Usuario actualizado", "OK");
        }
        public async Task<ApiResponse<string>> EliminarUsuarioAsync(int idUsuario)
        {
            var usuario = await _repo.GetUsuarioById(idUsuario);

            if (usuario == null)
                return new ApiResponse<string>(false, "Usuario no existe", null);

            if (usuario.Cuenta == null || usuario.Cuenta.Saldo > 0)
            {
                return new ApiResponse<string>(false, "No se puede eliminar usuario con saldo", null);
            }

            usuario.FlgEli = true;

            await _repo.SaveChangesAsync();

            return new ApiResponse<string>(true, "Usuario eliminado", "OK");
        }
        public async Task<ApiResponse<string>> EliminarGrupoAsync(int id)
        {
            var grupo = await _repo.GetGrupoByIdAsync(id);

            if (grupo == null)
                return new ApiResponse<string>(false, "Grupo no existe", null);

            grupo.FlgEli = true;

            await _repo.SaveChangesAsync();

            return new ApiResponse<string>(true, "Grupo eliminado", "OK");
        }
        public async Task<ApiResponse<GrupoDetalleDto>> ObtenerGrupoDetalleAsync(string codgrupo)
        {
            var data = await _repo.GetGrupoDetalleAsync(codgrupo);

            if (data == null)
                return new ApiResponse<GrupoDetalleDto>(false, "Grupo no encontrado", null);

            return new ApiResponse<GrupoDetalleDto>(true, "OK", data);
        }
    }
}

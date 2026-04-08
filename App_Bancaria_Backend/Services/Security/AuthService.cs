using App_Bancaria_Backend.DTOs.Base;
using App_Bancaria_Backend.DTOs.Security;
using App_Bancaria_Backend.Models;
using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.Repositories.Security;
using Microsoft.EntityFrameworkCore;

namespace App_Bancaria_Backend.Services.Security
{
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly AppDbContext _context;
        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public AuthService(IUsuarioRepository usuarioRepository, AppDbContext context)
        {
            _usuarioRepository = usuarioRepository;
            _context = context;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

            if (usuario == null)
                return new ApiResponse<LoginResponseDto>(false, "Usuario no encontrado", null);

            if (usuario.PasswordHash != request.Password)
                return new ApiResponse<LoginResponseDto>(false, "Contraseña incorrecta", null);

            var response = new LoginResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre + ' ' + usuario.ApePat +' '+ usuario.ApePat,
                Token = Guid.NewGuid().ToString()
            };

            return new ApiResponse<LoginResponseDto>(true, "Login exitoso", response);
        }

        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request)
        {

            // 🔹 Validar duplicados
            if (await _usuarioRepository.ExisteEmailAsync(request.Email))
                return new ApiResponse<RegisterResponseDto>(false, "El email ya existe", null);

            if (await _usuarioRepository.ExisteTelefonoAsync(request.NroTelefono))
                return new ApiResponse<RegisterResponseDto>(false, "El teléfono ya existe", null);

            // 🔹 Crear usuario
            var usuario = new Usuario
            {
                CodUsuario = Guid.NewGuid().ToString(),
                Nombre = request.Nombre,
                ApePat = request.ApePat,
                ApeMate = request.ApeMate,
                NroTelefono = request.NroTelefono,
                Email = request.Email,
                PasswordHash = request.Password,
                Token = request.Token,
                FlgEstado = false,
                FlgEli = false
            };

            await _usuarioRepository.AddAsync(usuario);

            // 🔹 Crear cuenta automáticamente
            var cuenta = new Cuenta
            {
                CodCuenta = Guid.NewGuid().ToString(),
                IdUsuario = usuario.IdUsuario,
                Saldo = 0,
                CodQR = Guid.NewGuid().ToString()
            };

            _context.Cuenta.Add(cuenta);
            await _context.SaveChangesAsync();

            // 🔹 Respuesta
            var response = new RegisterResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                NroTelefono = usuario.NroTelefono,
                CodCuenta = cuenta.CodCuenta
            };

            return new ApiResponse<RegisterResponseDto>(true, "Usuario registrado correctamente", response);
        }
    }
}

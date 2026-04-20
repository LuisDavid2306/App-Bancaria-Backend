using App_Bancaria_Backend.DTOs.Base;
using App_Bancaria_Backend.Repositories.Transacciones;

namespace App_Bancaria_Backend.Services.Users
{
    public class UsuarioService
    {
        private readonly ITransaccionesRepository _repo;

        public UsuarioService(ITransaccionesRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<decimal>> ObtenerSaldoAsync(int idUsuario)
        {
            var usuario = await _repo.GetUsuarioConCuenta(idUsuario);

            if (usuario == null)
                return new ApiResponse<decimal>(false, "Usuario no encontrado", 0);

            return new ApiResponse<decimal>(true, "Saldo obtenido", usuario.Cuenta.Saldo);
        }
    }
}

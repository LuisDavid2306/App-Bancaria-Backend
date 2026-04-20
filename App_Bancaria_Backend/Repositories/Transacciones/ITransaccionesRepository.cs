using App_Bancaria_Backend.DTOs.Base;
using App_Bancaria_Backend.DTOs.Transacciones;
using App_Bancaria_Backend.Models;

namespace App_Bancaria_Backend.Repositories.Transacciones
{
    public interface ITransaccionesRepository
    {
        Task<Usuario> GetUsuarioConCuenta(int idUsuario);
        Task<Usuario> GetUsuarioPorTelefono(string telefono);
        Task AddTransaccion(Transaccion transaccion);
        Task SaveChangesAsync();
        Task<TipoTransaccion> GetTipoTransaccionPorNombre(string nombre);
        Task<List<Transaccion>> GetMovimientosPorCuenta(int idCuenta);
    }
}

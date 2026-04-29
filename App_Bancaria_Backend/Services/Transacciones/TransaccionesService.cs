using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.DTOs.Base;
using App_Bancaria_Backend.DTOs.GrupoAhorros;
using App_Bancaria_Backend.DTOs.Transacciones;
using App_Bancaria_Backend.Models;
using App_Bancaria_Backend.Repositories.Transacciones;
using Microsoft.EntityFrameworkCore;

public class TransaccionesService
{
    private readonly ITransaccionesRepository _repo;
    private readonly AppDbContext _context;

    public TransaccionesService(ITransaccionesRepository repo, AppDbContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<ApiResponse<string>> TransferirAsync(int userId, TransferenciaDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var usuarioOrigen = await _repo.GetUsuarioConCuenta(userId);
            var usuarioDestino = await _repo.GetUsuarioPorTelefono(dto.NumeroTelefonoDestino);
            var tipo = await _repo.GetTipoTransaccionPorNombre("Transferencia");

            if (tipo == null)
                return new ApiResponse<string>(false, "Tipo de transacción no configurado", null);

            if (usuarioOrigen == null || usuarioDestino == null)
                return new ApiResponse<string>(false, "Usuario no encontrado", null);

            if (dto.Monto <= 0)
                return new ApiResponse<string>(false, "Monto Inválido", null);

            if (usuarioOrigen.Cuenta.Saldo < dto.Monto)
                return new ApiResponse<string>(false, "Saldo Insuficiente", null);

            usuarioOrigen.Cuenta.Saldo -= dto.Monto;
            usuarioDestino.Cuenta.Saldo += dto.Monto;

            var transaccionEntity = new Transaccion
            {
                CodTransaccion = Guid.NewGuid().ToString(),
                IdCuentaOrigen = usuarioOrigen.Cuenta.IdCuenta,
                IdCuentaDestino = usuarioDestino.Cuenta.IdCuenta,
                Monto = dto.Monto,
                IdTipoTransaccion = tipo.IdTipoTransaccion,
                Descripcion = dto.Descripcion,
                Fecha = DateTime.Now
            };

            await _repo.AddTransaccion(transaccionEntity);
            await _repo.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ApiResponse<string>(true, "Transferencia realizada", "OK");
        }
        catch
        {
            await transaction.RollbackAsync();
            return new ApiResponse<string>(false, "Error en la transacción", null);
        }
    }

    public async Task<ApiResponse<string>> RecargarAsync(int userId, RecargarDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var usuario = await _repo.GetUsuarioConCuenta(userId);

            if (usuario == null)
                return new ApiResponse<string>(false, "Usuario no encontrado", null);

            if (dto.Monto <= 0)
                return new ApiResponse<string>(false, "Monto inválido", null);

            var tipo = await _repo.GetTipoTransaccionPorNombre("Recarga");

            if (tipo == null)
                return new ApiResponse<string>(false, "Tipo de transacción no configurado", null);

            usuario.Cuenta.Saldo += dto.Monto;

            var transaccion = new Transaccion
            {
                CodTransaccion = Guid.NewGuid().ToString(),
                IdCuentaOrigen = usuario.Cuenta.IdCuenta,
                IdCuentaDestino = null, 
                Monto = dto.Monto,
                IdTipoTransaccion = tipo.IdTipoTransaccion,
                Descripcion = tipo.Nombre,
                Fecha = DateTime.Now
            };

            await _repo.AddTransaccion(transaccion);
            await _repo.SaveChangesAsync();

            await transaction.CommitAsync();

            return new ApiResponse<string>(true, "Recarga exitosa", "OK");
        }
        catch
        {
            await transaction.RollbackAsync();
            return new ApiResponse<string>(false, "Error en la recarga", null);
        }
    }

    public async Task<ApiResponse<List<HistorialDto>>> HistorialAsync(int userId)
    {
        var usuario = await _repo.GetUsuarioConCuenta(userId);

        if (usuario == null)
            return new ApiResponse<List<HistorialDto>>(false, "Usuario no encontrado", null);

        var movimientos = await _repo.GetMovimientosPorCuenta(usuario.Cuenta.IdCuenta);

        var historial = movimientos.Select(t => new HistorialDto
        {
            Tipo = t.TipoTransaccion.Nombre,
            Monto = t.Monto,
            Descripcion = t.Descripcion,
            Fecha = t.Fecha,
            EsIngreso = t.IdCuentaDestino == usuario.Cuenta.IdCuenta || t.IdTipoTransaccion == '4'
        }).ToList();

        return new ApiResponse<List<HistorialDto>>(true, "OK", historial);
    }

}
using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.DTOs.Transacciones;
using App_Bancaria_Backend.Models;
using App_Bancaria_Backend.Repositories.Transacciones;
using Microsoft.EntityFrameworkCore;

public class TransaccionesRepository : ITransaccionesRepository
{
    private readonly AppDbContext _context;

    public TransaccionesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> GetUsuarioConCuenta(int idUsuario)
    {
        return await _context.Usuario
            .Include(u => u.Cuenta)
            .FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);
    }

    public async Task<Usuario> GetUsuarioPorTelefono(string telefono)
    {
        return await _context.Usuario
            .Include(u => u.Cuenta)
            .FirstOrDefaultAsync(x => x.NroTelefono == telefono);
    }
    public async Task<TipoTransaccion> GetTipoTransaccionPorNombre(string nombre)
    {
        return await _context.TipoTransaccion
            .FirstOrDefaultAsync(x => x.Nombre == nombre);
    }
    public async Task<List<Transaccion>> GetMovimientosPorCuenta(int idCuenta)
    {
        return await _context.Transaccion
            .Include(t => t.TipoTransaccion)
            .Where(t => t.IdCuentaOrigen == idCuenta || t.IdCuentaDestino == idCuenta)
            .OrderByDescending(t => t.Fecha)
            .ToListAsync();
    }

    public async Task AddTransaccion(Transaccion transaccion)
    {
        await _context.Transaccion.AddAsync(transaccion);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    
}
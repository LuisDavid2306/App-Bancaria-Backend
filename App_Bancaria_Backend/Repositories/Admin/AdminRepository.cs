using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.DTOs.Admin;
using App_Bancaria_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Bancaria_Backend.Repositories.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioAdminDto>> GetUsuariosAsync()
        {
            return await _context.Usuario
                .Include(u => u.Cuenta)
                .Select(u => new UsuarioAdminDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre + " " + u.ApePat,
                    Email = u.Email,
                    Telefono = u.NroTelefono,
                    Saldo = u.Cuenta.Saldo
                })
                .ToListAsync();
        }
        public async Task<List<TransaccionAdminDto>> GetTransaccionesAsync()
        {
            return await _context.Transaccion
                .Include(t => t.TipoTransaccion)
                .Include(t => t.CuentaOrigen)
                    .ThenInclude(c => c.Usuario)
                .Include(t => t.CuentaDestino)
                    .ThenInclude(c => c.Usuario)
                .OrderByDescending(t => t.Fecha)
                .Select(t => new TransaccionAdminDto
                {
                    CodTransaccion = t.CodTransaccion,
                    Tipo = t.TipoTransaccion.Nombre,
                    Monto = t.Monto,
                    UsuarioOrigen = t.CuentaOrigen.Usuario.Nombre,
                    UsuarioDestino = t.CuentaDestino != null
                        ? t.CuentaDestino.Usuario.Nombre
                        : "N/A",
                    Fecha = t.Fecha
                })
                .ToListAsync();
        }
        public async Task<List<GrupoAdminDto>> GetGruposAsync()
        {
            return await _context.GrupoAhorro
                .Include(g => g.Miembros)
                .Select(g => new GrupoAdminDto
                {
                    CodGrupo = g.CodGrupo,
                    Nombre = g.Nombre,
                    MontoActual = g.MontoActual,
                    MontoObjetivo = g.MontoObjetivo,
                    CantidadMiembros = g.Miembros.Count,
                    Estado = g.Estado
                })
                .ToListAsync();
        }
        public async Task<DashboardDto> GetDashboardAsync()
        {
            var totalUsuarios = await _context.Usuario.CountAsync();
            var totalTransacciones = await _context.Transaccion.CountAsync();
            var totalGrupos = await _context.GrupoAhorro.CountAsync();

            var hoy = DateTime.Today;
            var hace7Dias = hoy.AddDays(-6);

            var data = await _context.Transaccion
                    .Where(t => t.Fecha >= hace7Dias)
                    .GroupBy(t => t.Fecha.Date)
                    .Select(g => new
                    {
                        Fecha = g.Key,
                        Cantidad = g.Count()
                    })
                    .OrderBy(x => x.Fecha)
                    .ToListAsync();

            var transaccionesPorDia = data.Select(x => new TransaccionesPorDiaDto
            {
                Fecha = x.Fecha.ToString("yyyy-MM-dd"), // ✅ aquí sí funciona
                Cantidad = x.Cantidad
            }).ToList();

            // 💰 dinero total en cuentas
            var totalDinero = await _context.Cuenta
                                .Select(c => (decimal?)c.Saldo)
                                .SumAsync() ?? 0;

            var dineroData = await _context.Transaccion
                    .Where(t => t.Fecha >= hace7Dias)
                    .GroupBy(t => t.Fecha.Date)
                    .Select(g => new
                    {
                        Fecha = g.Key,
                        Total = g.Sum(x => x.Monto)
                    })
                    .OrderBy(x => x.Fecha)
                    .ToListAsync(); 

            var dineroPorDia = dineroData.Select(x => new DineroPorDiaDto
            {
                Fecha = x.Fecha.ToString("yyyy-MM-dd"),
                Total = x.Total
            }).ToList();

            var topUsuariosData = await _context.Transaccion
                    .Include(t => t.CuentaOrigen)
                        .ThenInclude(c => c.Usuario)
                    .GroupBy(t => t.CuentaOrigen.Usuario)
                    .Select(g => new
                    {
                        Nombre = g.Key.Nombre + " " + g.Key.ApePat,
                        Total = g.Sum(x => x.Monto)
                    })
                    .OrderByDescending(x => x.Total)
                    .Take(5)
                    .ToListAsync();

            var topUsuarios = topUsuariosData.Select(x => new TopUsuarioDto
            {
                Nombre = x.Nombre,
                TotalMovido = x.Total
            }).ToList();

            return new DashboardDto
            {
                TotalUsuarios = totalUsuarios,
                TotalTransacciones = totalTransacciones,
                TotalGrupos = totalGrupos,
                TotalDineroSistema = totalDinero,
                TransaccionesPorDia = transaccionesPorDia,
                DineroPorDia = dineroPorDia,
                TopUsuarios = topUsuarios
            };
        }
        public async Task AddUsuarioAsync(Usuario usuario)
        {
            await _context.Usuario.AddAsync(usuario);
        }

        public async Task<Usuario> GetUsuarioById(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }

        public Task DeleteUsuarioAsync(Usuario usuario)
        {
            _context.Usuario.Remove(usuario);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<GrupoAhorro> GetGrupoByIdAsync(int id)
        {
            return await _context.GrupoAhorro
                .FirstOrDefaultAsync(x => x.IdGrupo == id && !x.FlgEli);
        }

        public Task DeleteGrupoAsync(GrupoAhorro grupo)
        {
            grupo.FlgEli = true;
            return Task.CompletedTask;
        }
    }
}

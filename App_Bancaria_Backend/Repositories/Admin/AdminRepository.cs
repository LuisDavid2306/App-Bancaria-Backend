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

        public async Task<List<UsuarioAdminDto>> GetUsuariosAsync(string? search)
        {
            var query = _context.Usuario
                .Include(u => u.Cuenta)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u =>
                    u.Nombre.Contains(search) ||
                    u.Email.Contains(search));
            }

            return await query
                .Select(u => new UsuarioAdminDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Telefono = u.NroTelefono,
                    Saldo = u.Cuenta.Saldo
                })
                .ToListAsync();
        }
        public async Task<List<TransaccionAdminDto>> GetTransaccionesAsync(DateTime? inicio, DateTime? fin, string? tipo, string? usuario)
        {
            var query = _context.Transaccion
                .Include(t => t.TipoTransaccion)
                .Include(t => t.CuentaOrigen).ThenInclude(c => c.Usuario)
                .Include(t => t.CuentaDestino).ThenInclude(c => c.Usuario)
                .AsQueryable();

            if (inicio.HasValue)
                query = query.Where(x => x.Fecha >= inicio);

            if (fin.HasValue)
                query = query.Where(x => x.Fecha <= fin);

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(x => x.TipoTransaccion.Nombre.Contains(tipo));

            if (!string.IsNullOrEmpty(usuario))
                query = query.Where(x =>
                    x.CuentaOrigen.Usuario.Nombre.Contains(usuario) ||
                    x.CuentaDestino.Usuario.Nombre.Contains(usuario));

            return await query
                .OrderByDescending(x => x.Fecha)
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
                .Where(g => g.FlgEli == false)
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
        public async Task<DashboardDto> GetDashboardAsync(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var totalUsuarios = await _context.Usuario.CountAsync();
            var totalTransacciones = await _context.Transaccion.CountAsync();
            var totalGrupos = await _context.GrupoAhorro.CountAsync();

            var query = _context.Transaccion.AsQueryable();

            if (fechaInicio.HasValue)
            {
                query = query.Where(t => t.Fecha >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(t => t.Fecha <= fechaFin.Value);
            }

            var data = await query
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
                Fecha = x.Fecha.ToString("yyyy-MM-dd"),
                Cantidad = x.Cantidad
            }).ToList();

            
            var totalDinero = await _context.Cuenta
                                .Select(c => (decimal?)c.Saldo)
                                .SumAsync() ?? 0;

            var dineroData = await query
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

            var topUsuariosData = await query
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

        public async Task<GrupoDetalleDto> GetGrupoDetalleAsync(string codgrupo)
        {
            var grupo = await _context.GrupoAhorro
                .Include(g => g.Miembros)
                    .ThenInclude(m => m.Usuario)
                .Include(g => g.Aportes)
                    .ThenInclude(a => a.Usuario)
                .Include(g => g.Retiros)
                    .ThenInclude(r => r.Usuario)
                .FirstOrDefaultAsync(g => g.CodGrupo == codgrupo && !g.FlgEli);

            if (grupo == null)
                return null;

            return new GrupoDetalleDto
            {
                CodGrupo = grupo.CodGrupo,
                Nombre = grupo.Nombre,
                MontoActual = grupo.MontoActual,
                MontoObjetivo = grupo.MontoObjetivo,
                Progreso = grupo.MontoObjetivo > 0
                                ? (grupo.MontoActual / grupo.MontoObjetivo) * 100
                                : 0,

                Miembros = grupo.Miembros.Select(m => new MiembroDto
                {
                    Nombre = m.Usuario != null ? m.Usuario.Nombre : "N/A",
                    FechaUnion = m.FechaUnion
                }).ToList(),

                Aportes = grupo.Aportes.OrderByDescending(a => a.Fecha)
                .Select(a => new AporteDto
                {
                    Usuario = a.Usuario != null ? a.Usuario.Nombre : "N/A",
                    Monto = a.Monto,
                    Fecha = a.Fecha
                }).ToList(),

                Retiros = grupo.Retiros
                .OrderByDescending(r => r.FechaSolicitud)
                .Select(r => new RetiroDto
                {
                    Usuario = r.Usuario != null ? r.Usuario.Nombre : "N/A",
                    Monto = r.Monto,
                    Fecha = r.FechaSolicitud,
                    Estado = r.Estado
                })
                .ToList(),
            };
        }
    }
}

using App_Bancaria_Backend.Data;
using App_Bancaria_Backend.DTOs.Base;
using App_Bancaria_Backend.DTOs.GrupoAhorros;
using App_Bancaria_Backend.DTOs.Transacciones;
using App_Bancaria_Backend.Models;
using App_Bancaria_Backend.Repositories.GrupoAhorros;
using App_Bancaria_Backend.Repositories.Transacciones;
using Microsoft.EntityFrameworkCore;

namespace App_Bancaria_Backend.Services.GrupoAhorros
{
    public class GrupoAhorrosService
    {
        private readonly IGrupoAhorroRepository _repo;
        private readonly ITransaccionesRepository _transRepo;
        private readonly AppDbContext _context;

        public GrupoAhorrosService(IGrupoAhorroRepository repo, ITransaccionesRepository transRepo, AppDbContext context)
        {
            _repo = repo;
            _transRepo = transRepo;
            _context = context;
        }

        public async Task<ApiResponse<string>> CrearGrupoAsync(int userId, CrearGrupoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var usuario = await _repo.GetUsuarioConCuenta(userId);

            if (dto.MontoObjetivo <= 0)
                return new ApiResponse<string>(false, "Ingrese un objetivo válido.", null);

            try
            {
                var grupo = new GrupoAhorro
                {
                    CodGrupo = Guid.NewGuid().ToString(),
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    MontoObjetivo = dto.MontoObjetivo,
                    MontoActual = 0,
                    FechaCreacion = DateTime.Now,
                    Estado = "Activo"
                };

                await _repo.AddGrupoAsync(grupo);
                await _repo.SaveChangesAsync();

                var miembro = new GrupoMiembro
                {
                    CodGrupoMiembro = Guid.NewGuid().ToString(),
                    IdGrupo = grupo.IdGrupo,
                    IdUsuario = userId,
                    FechaUnion = DateTime.Now
                };

                await _repo.AddMiembroAsync(miembro);

                var admin = new GrupoAdministrador
                {
                    CodGrupoAdmin = Guid.NewGuid().ToString(),
                    IdGrupo = grupo.IdGrupo,
                    IdUsuario = userId
                };

                await _repo.AddAdministradorAsync(admin);

                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiResponse<string>(true, "Grupo creado correctamente", grupo.CodGrupo);
            }
            catch
            {
                await transaction.RollbackAsync();
                return new ApiResponse<string>(false, "Error al crear grupo", null);
            }
        }

        public async Task<ApiResponse<string>> UnirseGrupoAsync(int userId, UnirseGrupoDto dto)
        {
            try
            {
                var grupo = await _repo.GetGrupoById(dto.IdGrupo);

                if (grupo == null)
                    return new ApiResponse<string>(false, "Grupo no existe", null);

                var yaExiste = await _repo.ExisteMiembro(userId, dto.IdGrupo);

                if (yaExiste)
                    return new ApiResponse<string>(false, "Ya perteneces a este grupo", null);

                var miembro = new GrupoMiembro
                {
                    CodGrupoMiembro = Guid.NewGuid().ToString(),
                    IdGrupo = dto.IdGrupo,
                    IdUsuario = userId,
                    FechaUnion = DateTime.Now
                };

                await _repo.AddMiembroAsync(miembro);
                await _repo.SaveChangesAsync();

                return new ApiResponse<string>(true, "Te uniste al grupo", "OK");
            }
            catch
            {
                return new ApiResponse<string>(false, "Error al unirse", null);
            }
        }

        public async Task<ApiResponse<string>> AportarAsync(int userId, AportarGrupoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _repo.GetUsuarioConCuenta(userId);
                var grupo = await _repo.GetGrupoById(dto.IdGrupo);

                if (usuario == null || grupo == null)
                    return new ApiResponse<string>(false, "Usuario o grupo no existe", null);

                if (dto.Monto <= 0)
                    return new ApiResponse<string>(false, "Monto inválido", null);

                var esMiembro = await _repo.EsMiembro(userId, dto.IdGrupo);

                if (!esMiembro)
                    return new ApiResponse<string>(false, "No perteneces al grupo", null);

                if (usuario.Cuenta.Saldo < dto.Monto)
                    return new ApiResponse<string>(false, "Saldo insuficiente", null);

                var tipo = await _repo.GetTipoTransaccionPorNombre("Aporte");

                if (tipo == null)
                    return new ApiResponse<string>(false, "Tipo de transacción no configurado", null);

                usuario.Cuenta.Saldo -= dto.Monto;

                grupo.MontoActual += dto.Monto;

                var transaccion = new Transaccion
                {
                    CodTransaccion = Guid.NewGuid().ToString(),
                    IdCuentaOrigen = usuario.Cuenta.IdCuenta,
                    IdCuentaDestino = null,
                    Monto = dto.Monto,
                    IdTipoTransaccion = tipo.IdTipoTransaccion,
                    Descripcion = "Aporte a grupo",
                    Fecha = DateTime.Now
                };

                await _transRepo.AddTransaccion(transaccion);
                await _repo.SaveChangesAsync();

                var aporte = new Aporte
                {
                    CodAporte = Guid.NewGuid().ToString(),
                    IdTransaccion = transaccion.IdTransaccion,
                    IdGrupo = dto.IdGrupo,
                    IdUsuario = userId,
                    Monto = dto.Monto,
                    Fecha = DateTime.Now
                };

                await _repo.AddAporteAsync(aporte);

                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiResponse<string>(true, "Aporte realizado", "OK");
            }
            catch
            {
                await transaction.RollbackAsync();
                return new ApiResponse<string>(false, "Error en el aporte", null);
            }
        }

        public async Task<ApiResponse<string>> SolicitarRetiroAsync(int userId, SolicitarRetiroDto dto)
        {
            try
            {
                var grupo = await _repo.GetGrupoById(dto.IdGrupo);

                if (grupo == null)
                    return new ApiResponse<string>(false, "Grupo no existe", null);

                if (dto.Monto <= 0)
                    return new ApiResponse<string>(false, "Monto inválido", null);

                if (grupo.MontoActual < dto.Monto)
                    return new ApiResponse<string>(false, "Fondos insuficientes en el grupo", null);

                var esAdmin = await _context.GrupoAdministrador
                                .AnyAsync(x => x.IdGrupo == dto.IdGrupo && x.IdUsuario == userId);
                
                if (!esAdmin)
                    return new ApiResponse<string>(false, "Solo administradores pueden solicitar retiro", null);

                var existePendiente = await _context.Retiro
                                   .AnyAsync(x => x.IdGrupo == dto.IdGrupo && x.Estado == "Pendiente");

                if (existePendiente)
                    return new ApiResponse<string>(false, "Ya existe un retiro pendiente en este grupo", null);

                if (grupo.Estado != "Activo")
                    return new ApiResponse<string>(false, "El grupo no está activo", null);

                var usuario = await _repo.GetUsuarioConCuenta(userId);

                if (usuario?.Cuenta == null)
                    return new ApiResponse<string>(false, "Usuario sin cuenta válida", null);

                var retiro = new Retiro
                {
                    CodRetiro = Guid.NewGuid().ToString(),
                    IdGrupo = dto.IdGrupo,
                    IdUsuarioSolicitante = userId,
                    Monto = dto.Monto,
                    Estado = "Pendiente",
                    FechaSolicitud = DateTime.Now
                };

                await _repo.AddRetiroAsync(retiro);
                await _repo.SaveChangesAsync();

                return new ApiResponse<string>(true, "Solicitud de retiro creada", retiro.CodRetiro);
            }
            catch
            {
                return new ApiResponse<string>(false, "Error al solicitar retiro", null);
            }
        }

        public async Task<ApiResponse<string>> AprobarRetiroAsync(int userId, AprobarRetiroDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var retiro = await _repo.GetRetiroById(dto.IdRetiro);

                if (retiro == null)
                    return new ApiResponse<string>(false, "Retiro no existe", null);

                if (retiro.Estado != "Pendiente")
                    return new ApiResponse<string>(false, "El retiro ya fue procesado", null);

                var esAdmin = await _context.GrupoAdministrador
                    .AnyAsync(x => x.IdGrupo == retiro.IdGrupo && x.IdUsuario == userId);

                if (!esAdmin)
                    return new ApiResponse<string>(false, "No eres administrador", null);

                var yaAprobo = await _repo.YaAprobo(userId, dto.IdRetiro);

                if (yaAprobo)
                    return new ApiResponse<string>(false, "Ya aprobaste este retiro", null);

                var aprobacion = new RetiroAprobacion
                {
                    CodAprobacion = Guid.NewGuid().ToString(),
                    IdRetiro = dto.IdRetiro,
                    IdUsuario = userId,
                    Aprobado = dto.Aprobado,
                    Fecha = DateTime.Now
                };

                await _repo.AddAprobacionAsync(aprobacion);
                await _repo.SaveChangesAsync();

                if (!dto.Aprobado)
                {
                    retiro.Estado = "Rechazado";
                    await _repo.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiResponse<string>(true, "Retiro rechazado", "OK");
                }

                var totalAdmins = await _repo.CantidadAdmins(retiro.IdGrupo);
                var totalAprobaciones = await _repo.CantidadAprobaciones(dto.IdRetiro);

                if (totalAdmins == totalAprobaciones)
                {
                    var grupo = await _repo.GetGrupoById(retiro.IdGrupo);

                    var usuarioSolicitante = await _context.Usuario
                        .Include(u => u.Cuenta)
                        .FirstOrDefaultAsync(x => x.IdUsuario == retiro.IdUsuarioSolicitante);

                    if (grupo == null || usuarioSolicitante == null)
                        return new ApiResponse<string>(false, "Error al procesar retiro", null);

                    grupo.MontoActual -= retiro.Monto;
                    usuarioSolicitante.Cuenta.Saldo += retiro.Monto;

                    retiro.Estado = "Aprobado";

                    await _repo.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return new ApiResponse<string>(true, "Aprobación registrada", "OK");
            }
            catch
            {
                await transaction.RollbackAsync();
                return new ApiResponse<string>(false, "Error al aprobar retiro", null);
            }
        }

        public async Task<ApiResponse<List<MisGruposDto>>> ObtenerMisGruposAsync(int userId)
        {
            var grupos = await _context.GrupoMiembro
                .Where(gm => gm.IdUsuario == userId)
                .Select(gm => new MisGruposDto
                {
                    IdGrupo = gm.GrupoAhorro.IdGrupo,
                    Nombre = gm.GrupoAhorro.Nombre,
                    MontoActual = gm.GrupoAhorro.MontoActual,
                    MontoObjetivo = gm.GrupoAhorro.MontoObjetivo
                })
                .ToListAsync();

            return new ApiResponse<List<MisGruposDto>>(true, "OK", grupos);
        }
    }
}

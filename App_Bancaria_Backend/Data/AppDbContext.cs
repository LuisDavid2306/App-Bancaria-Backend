using App_Bancaria_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Bancaria_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 🔹 DbSets (todas tus tablas)
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<Transaccion> Transaccion { get; set; }
        public DbSet<TipoTransaccion> TipoTransaccion { get; set; }
        public DbSet<GrupoAhorro> GrupoAhorro { get; set; }
        public DbSet<GrupoMiembro> GrupoMiembro { get; set; }
        public DbSet<GrupoAdministrador> GrupoAdministrador { get; set; }
        public DbSet<Aporte> Aporte { get; set; }
        public DbSet<Retiro> Retiro { get; set; }
        public DbSet<RetiroAprobacion> RetiroAprobacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TipoTransaccion>().HasData(
                new TipoTransaccion { IdTipoTransaccion = 1, CodTipoTransaccion = "T001", Nombre = "Transferencia" },
                new TipoTransaccion { IdTipoTransaccion = 2, CodTipoTransaccion = "T002", Nombre = "Aporte" },
                new TipoTransaccion { IdTipoTransaccion = 3, CodTipoTransaccion = "T003", Nombre = "Retiro" },
                new TipoTransaccion { IdTipoTransaccion = 4, CodTipoTransaccion = "T004", Nombre = "Recarga" }
            );

            modelBuilder.Entity<Retiro>()
            .HasOne(r => r.Usuario)
            .WithMany()
            .HasForeignKey(r => r.IdUsuarioSolicitante)
            .OnDelete(DeleteBehavior.NoAction);

            // 🔥 1. Relación 1:1 Usuario - Cuenta
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Cuenta)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Cuenta>(c => c.IdUsuario);

            // 🔥 2. Transacción → CuentaOrigen (1:N)
            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.CuentaOrigen)
                .WithMany(c => c.TransaccionesOrigen)
                .HasForeignKey(t => t.IdCuentaOrigen)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔥 3. Transacción → CuentaDestino (1:N opcional)
            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.CuentaDestino)
                .WithMany(c => c.TransaccionesDestino)
                .HasForeignKey(t => t.IdCuentaDestino)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔥 4. Índices únicos (reflejan tu BD)
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NroTelefono)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.CodUsuario)
                .IsUnique();

            modelBuilder.Entity<Cuenta>()
                .HasIndex(c => c.CodCuenta)
                .IsUnique();

            modelBuilder.Entity<Cuenta>()
                .HasIndex(c => c.CodQR)
                .IsUnique();

            // 🔥 5. Defaults (opcional pero recomendado)
            modelBuilder.Entity<Usuario>()
                .Property(u => u.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Cuenta>()
                .Property(c => c.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Fecha)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Aporte>()
                .Property(a => a.Fecha)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Retiro>()
                .Property(r => r.FechaSolicitud)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<RetiroAprobacion>()
                .Property(r => r.Fecha)
                .HasDefaultValueSql("GETDATE()");

            // 🔥 Configuración de decimales (CRÍTICO en apps financieras)
            modelBuilder.Entity<Cuenta>()
                .Property(c => c.Saldo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Aporte>()
                .Property(a => a.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Retiro>()
                .Property(r => r.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GrupoAhorro>()
                .Property(g => g.MontoObjetivo)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GrupoAhorro>()
                .Property(g => g.MontoActual)
                .HasPrecision(18, 2);
        }
    }
}

using App_Bancaria_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace App_Bancaria_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        //public DbSet<Cuenta> Cuenta { get; set; }
        //public DbSet<Transaccion> Transaccion { get; set; }
        //public DbSet<TipoTransaccion> TipoTransaccion { get; set; }
        //public DbSet<GrupoAhorro> GrupoAhorro { get; set; }
        //public DbSet<GrupoMiembro> GrupoMiembro { get; set; }
        //public DbSet<GrupoAdministrador> GrupoAdministrador { get; set; }
        //public DbSet<Aporte> Aporte { get; set; }
        //public DbSet<Retiro> Retiro { get; set; }
        //public DbSet<RetiroAprobacion> RetiroAprobacion { get; set; }
    }
}

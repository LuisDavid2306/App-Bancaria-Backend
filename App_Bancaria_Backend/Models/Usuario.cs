using System.ComponentModel.DataAnnotations;

namespace App_Bancaria_Backend.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        public string CodUsuario { get; set; }
        public string Token { get; set; }
        public string Nombre { get; set; }
        public string ApePat { get; set; }
        public string ApeMate { get; set; }
        public string NroTelefono { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool FlgEstado { get; set; }
        public bool FlgEli { get; set; }

        // Relaciones
        public virtual Cuenta Cuenta { get; set; }
        public virtual ICollection<Aporte> Aportes { get; set; }
        public virtual ICollection<GrupoMiembro> GruposMiembro { get; set; }
        public virtual ICollection<GrupoAdministrador> GruposAdmin { get; set; }
        public virtual ICollection<RetiroAprobacion> Aprobaciones { get; set; }
    }
}

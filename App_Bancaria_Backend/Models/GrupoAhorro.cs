using System.ComponentModel.DataAnnotations;

namespace App_Bancaria_Backend.Models
{
    public class GrupoAhorro
    {
        [Key]
        public int IdGrupo { get; set; }

        public string CodGrupo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal MontoObjetivo { get; set; }
        public decimal MontoActual { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public bool FlgEli { get; set; } = false;

        public virtual ICollection<GrupoMiembro> Miembros { get; set; }
        public virtual ICollection<GrupoAdministrador> Administradores { get; set; }
        public virtual ICollection<Aporte> Aportes { get; set; }
        public virtual ICollection<Retiro> Retiros { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Bancaria_Backend.Models
{
    public class GrupoMiembro
    {
        [Key]
        public int IdGrupoMiembro { get; set; }

        public string CodGrupoMiembro { get; set; }

        [ForeignKey("GrupoAhorro")]
        public int IdGrupo { get; set; }
        public virtual GrupoAhorro GrupoAhorro { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public DateTime FechaUnion { get; set; }
    }
}

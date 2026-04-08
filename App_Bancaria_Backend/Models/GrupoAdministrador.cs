using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Bancaria_Backend.Models
{
    public class GrupoAdministrador
    {
        [Key]
        public int IdGrupoAdmin { get; set; }

        public string CodGrupoAdmin { get; set; }

        [ForeignKey("GrupoAhorro")]
        public int IdGrupo { get; set; }
        public virtual GrupoAhorro GrupoAhorro { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}

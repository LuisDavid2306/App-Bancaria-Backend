using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Bancaria_Backend.Models
{
    public class Retiro
    {
        [Key]
        public int IdRetiro { get; set; }
        public string CodRetiro { get; set; }

        [ForeignKey("GrupoAhorro")]
        public int IdGrupo { get; set; }
        public virtual GrupoAhorro GrupoAhorro { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }

        public int? IdUsuarioSolicitante { get; set; }

        [ForeignKey("IdUsuarioSolicitante")]
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<RetiroAprobacion> Aprobaciones { get; set; }
    }
}

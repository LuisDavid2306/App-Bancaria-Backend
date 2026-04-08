using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Bancaria_Backend.Models
{
    public class RetiroAprobacion
    {
        [Key]
        public int IdAprobacion { get; set; }

        public string CodAprobacion { get; set; }

        [ForeignKey("Retiro")]
        public int IdRetiro { get; set; }
        public virtual Retiro Retiro { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public bool Aprobado { get; set; }
        public DateTime Fecha { get; set; }
    }
}

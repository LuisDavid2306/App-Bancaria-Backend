using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Bancaria_Backend.Models
{
    public class Aporte
    {
        [Key]
        public int IdAporte { get; set; }
        public string CodAporte { get; set; }

        [ForeignKey ("Transaccion")]
        public int IdTransaccion { get; set; }
        public virtual Transaccion Transaccion { get; set; }

        [ForeignKey("GrupoAhorro")]
        public int IdGrupo { get; set; }
        public virtual GrupoAhorro GrupoAhorro { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public decimal Monto { get; set; }
        public DateTime fecha { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Bancaria_Backend.Models
{
    public class Cuenta
    {
        [Key]
        public int IdCuenta { get; set; }

        public string CodCuenta { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public decimal Saldo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CodQR { get; set; }
        public virtual ICollection<Transaccion> TransaccionesOrigen { get; set; }
        public virtual ICollection<Transaccion> TransaccionesDestino { get; set; }
    }
}

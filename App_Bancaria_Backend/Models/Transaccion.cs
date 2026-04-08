using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App_Bancaria_Backend.Models
{
    public class Transaccion
    {
        [Key]
        public int IdTransaccion { get; set; }

        public string CodTransaccion { get; set; }

        [ForeignKey("CuentaOrigen")]
        public int IdCuentaOrigen { get; set; }
        public virtual Cuenta CuentaOrigen { get; set; }

        [ForeignKey("CuentaDestino")]
        public int? IdCuentaDestino { get; set; }
        public virtual Cuenta CuentaDestino { get; set; }

        public decimal Monto { get; set; }

        [ForeignKey("TipoTransaccion")]
        public int IdTipoTransaccion { get; set; }
        public virtual TipoTransaccion TipoTransaccion { get; set; }

        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public virtual ICollection<Aporte> Aportes { get; set; }
    }
}

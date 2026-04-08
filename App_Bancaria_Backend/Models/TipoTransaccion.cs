using System.ComponentModel.DataAnnotations;

namespace App_Bancaria_Backend.Models
{
    public class TipoTransaccion
    {
        [Key]
        public int IdTipoTransaccion { get; set; }

        public string CodTipoTransaccion { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Transaccion> Transacciones { get; set; }
    }
}


namespace App_Bancaria_Backend.DTOs.Transacciones
{
    public class TransferenciaDto
    {
        public string NumeroTelefonoDestino { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
    }
}

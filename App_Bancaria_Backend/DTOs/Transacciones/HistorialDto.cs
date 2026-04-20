namespace App_Bancaria_Backend.DTOs.Transacciones
{
    public class HistorialDto
    {
        public string Tipo { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public bool EsIngreso { get; set; }
    }
}

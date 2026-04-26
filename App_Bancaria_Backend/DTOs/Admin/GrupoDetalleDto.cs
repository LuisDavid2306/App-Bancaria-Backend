namespace App_Bancaria_Backend.DTOs.Admin
{
    public class GrupoDetalleDto
    {
        public string CodGrupo { get; set; }
        public string Nombre { get; set; }
        public decimal MontoActual { get; set; }
        public decimal MontoObjetivo { get; set; }
        public decimal Progreso { get; set; }
        public List<MiembroDto> Miembros { get; set; } = new();
        public List<AporteDto> Aportes { get; set; } = new();
        public List<RetiroDto> Retiros { get; set; } = new();
    }

    public class MiembroDto
    {
        public string Nombre { get; set; }
        public DateTime FechaUnion { get; set; }
    }

    public class AporteDto
    {
        public string Usuario { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class RetiroDto
    {
        public string Usuario { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}

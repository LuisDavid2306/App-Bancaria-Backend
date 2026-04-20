namespace App_Bancaria_Backend.DTOs.Admin
{
    public class DashboardDto
    {
        public int TotalUsuarios { get; set; }
        public int TotalTransacciones { get; set; }
        public int TotalGrupos { get; set; }
        public decimal TotalDineroSistema { get; set; }
        public List<TransaccionesPorDiaDto> TransaccionesPorDia { get; set; }
        public List<DineroPorDiaDto> DineroPorDia { get; set; }
        public List<TopUsuarioDto> TopUsuarios { get; set; }
    }
}

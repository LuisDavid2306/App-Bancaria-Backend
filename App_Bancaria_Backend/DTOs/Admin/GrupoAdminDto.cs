namespace App_Bancaria_Backend.DTOs.Admin
{
    public class GrupoAdminDto
    {
        public string CodGrupo { get; set; }
        public string Nombre { get; set; }
        public decimal MontoActual { get; set; }
        public decimal MontoObjetivo { get; set; }
        public int CantidadMiembros { get; set; }
        public string Estado { get; set; }
    }
}

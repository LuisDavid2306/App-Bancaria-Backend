namespace App_Bancaria_Backend.DTOs.GrupoAhorros
{
    public class MisGruposDto
    {
        public int IdGrupo { get; set; }
        public string Nombre { get; set; }
        public decimal MontoActual { get; set; }
        public decimal MontoObjetivo { get; set; }
    }
}

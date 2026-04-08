namespace App_Bancaria_Backend.DTOs.Security
{
    public class RegisterResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string NroTelefono { get; set; }
        public string CodCuenta { get; set; }
    }
}

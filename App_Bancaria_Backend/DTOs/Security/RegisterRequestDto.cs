namespace App_Bancaria_Backend.DTOs.Security
{
    public class RegisterRequestDto
    {
        public string Nombre { get; set; }
        public string ApePat { get; set; }
        public string ApeMate { get; set; }
        public string NroTelefono { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public string Token { get; set; }
    }
}

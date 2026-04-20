namespace App_Bancaria_Backend.DTOs.Admin
{
    public class EditarUsuarioAdminDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApePat { get; set; }
        public string ApeMate { get; set; }
        public string NroTelefono { get; set; }
    }
}

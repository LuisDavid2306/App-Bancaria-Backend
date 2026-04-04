using App_Bancaria_Backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace App_Bancaria_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var usuarios = _context.Usuario.ToList();
            return Ok(usuarios);
        }
    }
}

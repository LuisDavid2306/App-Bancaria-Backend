using App_Bancaria_Backend.DTOs.Admin;
using App_Bancaria_Backend.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App_Bancaria_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _service;

        public AdminController(AdminService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var result = await _service.ObtenerUsuariosAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("transacciones")]
        public async Task<IActionResult> GetTransacciones()
        {
            var result = await _service.ObtenerTransaccionesAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("grupos")]
        public async Task<IActionResult> GetGrupos()
        {
            var result = await _service.ObtenerGruposAsync();
            return Ok(result);
        }

        [HttpDelete("grupos/{id}")]
        public async Task<IActionResult> EliminarGrupo(int id)
        {
            var result = await _service.EliminarGrupoAsync(id);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _service.ObtenerDashboardAsync();
            return Ok(result);
        }
        [HttpPost("usuarios")]
        public async Task<IActionResult> CrearUsuario(CrearUsuarioAdminDto dto)
        {
            var result = await _service.CrearUsuarioAsync(dto);
            return Ok(result);
        }

        [HttpPut("usuarios")]
        public async Task<IActionResult> EditarUsuario(EditarUsuarioAdminDto dto)
        {
            var result = await _service.EditarUsuarioAsync(dto);
            return Ok(result);
        }

        [HttpDelete("usuarios/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var result = await _service.EliminarUsuarioAsync(id);
            return Ok(result);
        }
    }
}

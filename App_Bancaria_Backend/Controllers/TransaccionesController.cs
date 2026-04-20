using App_Bancaria_Backend.DTOs.Transacciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App_Bancaria_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionesController : ControllerBase
    {
        private readonly TransaccionesService _service;

        public TransaccionesController(TransaccionesService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("transferir")]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.TransferirAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("recargar")]
        public async Task<IActionResult> Recargar([FromBody] RecargarDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.RecargarAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("historial")]
        public async Task<IActionResult> Historial()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.HistorialAsync(userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

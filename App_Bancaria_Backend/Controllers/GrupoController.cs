using App_Bancaria_Backend.DTOs.GrupoAhorros;
using App_Bancaria_Backend.Services.GrupoAhorros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App_Bancaria_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrupoController : ControllerBase
    {
        private readonly GrupoAhorrosService _service;

        public GrupoController(GrupoAhorrosService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("crear")]
        public async Task<IActionResult> CrearGrupo([FromBody] CrearGrupoDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.CrearGrupoAsync(userId,dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("unirse")]
        public async Task<IActionResult> UnirseGrupo([FromBody] UnirseGrupoDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.UnirseGrupoAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("aportar")]
        public async Task<IActionResult> Aportar([FromBody] AportarGrupoDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.AportarAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("solicitar-retiro")]
        public async Task<IActionResult> SolicitarRetiro([FromBody] SolicitarRetiroDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.SolicitarRetiroAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("aprobar-retiro")]
        public async Task<IActionResult> AprobarRetiro([FromBody] AprobarRetiroDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.AprobarRetiroAsync(userId, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("mis-grupos")]
        public async Task<IActionResult> MisGrupos()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Token inválido");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.ObtenerMisGruposAsync(userId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

using App_Bancaria_Backend.Services;
using App_Bancaria_Backend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("saldo")]
    public async Task<IActionResult> ObtenerSaldo()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized("Token inválido");

        var userId = int.Parse(userIdClaim.Value);

        var result = await _service.ObtenerSaldoAsync(userId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
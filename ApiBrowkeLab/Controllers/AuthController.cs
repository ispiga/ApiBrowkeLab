using Microsoft.AspNetCore.Mvc;
using ApiBrowkeLab.DTOs;
using ApiBrowkeLab.Services;

namespace ApiBrowkeLab.Controllers;

/// <summary>
/// Controlador para manejar autenticación (registro y login)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Registra un nuevo usuario
    /// </summary>
    /// <param name="request">Datos del nuevo usuario (username, email, password)</param>
    /// <returns>Respuesta con éxito o error del registro</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation($"Intentando registrar usuario: {request.Email}");
        var response = await _authService.RegisterAsync(request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Autentica un usuario (login)
    /// </summary>
    /// <param name="request">Credenciales del usuario (email, password)</param>
    /// <returns>Respuesta con éxito y nombre de usuario o error</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation($"Intentando login de usuario: {request.Email}");
        var response = await _authService.LoginAsync(request);

        if (!response.Success)
        {
            return Unauthorized(response);
        }

        // Devolver únicamente el nombre de usuario correspondiente al email
        return Ok(new { username = response.Username });
    }
}

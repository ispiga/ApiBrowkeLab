namespace ApiBrowkeLab.DTOs;

/// <summary>
/// DTO para la solicitud de login
/// </summary>
public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

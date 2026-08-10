namespace ApiBrowkeLab.DTOs;

/// <summary>
/// DTO para la respuesta de autenticación (registro y login)
/// </summary>
public class AuthResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Email { get; set; }
}

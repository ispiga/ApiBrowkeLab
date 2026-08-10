using ApiBrowkeLab.Data;
using ApiBrowkeLab.DTOs;
using ApiBrowkeLab.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace ApiBrowkeLab.Services;

/// <summary>
/// Implementación del servicio de autenticación
/// </summary>
public class AuthService : IAuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Registra un nuevo usuario
    /// </summary>
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Validar que el email no exista
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "El email ya está registrado"
                };
            }

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Username, email y password son requeridos"
                };
            }

            // Hashear la contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Crear nuevo usuario
            var newUser = new User
            {
                Username = request.Username.Trim(),
                Email = request.Email.Trim(),
                PasswordHash = passwordHash
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Message = "Usuario registrado exitosamente",
                Username = newUser.Username,
                Email = newUser.Email
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"Error al registrar el usuario: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Autentica un usuario mediante email y contraseña
    /// </summary>
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email y password son requeridos"
                };
            }

            // Buscar usuario por email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email.Trim());

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email o contraseña incorrectos"
                };
            }

            // Verificar la contraseña
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email o contraseña incorrectos"
                };
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Login exitoso",
                Username = user.Username,
                Email = user.Email
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = $"Error al autenticar: {ex.Message}"
            };
        }
    }
}

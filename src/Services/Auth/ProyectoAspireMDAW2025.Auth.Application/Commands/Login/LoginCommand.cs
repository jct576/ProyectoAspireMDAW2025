using MediatR;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Auth;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Login;

/// <summary>
/// Command para login de usuario
/// </summary>
public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponse>;


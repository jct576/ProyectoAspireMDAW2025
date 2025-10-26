using MediatR;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Auth;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Register;

/// <summary>
/// Command para registrar un nuevo usuario
/// </summary>
public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword
) : IRequest<AuthResponse>;


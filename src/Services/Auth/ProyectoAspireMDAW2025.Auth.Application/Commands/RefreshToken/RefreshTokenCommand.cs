using MediatR;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Auth;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.RefreshToken;

/// <summary>
/// Command para refrescar el access token usando refresh token
/// </summary>
public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<AuthResponse>;


using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Auth;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.RefreshToken;

/// <summary>
/// Handler para RefreshTokenCommand
/// </summary>
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IAuthRepository authRepository,
        ITokenService tokenService,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando refresh de token");

        // Validar refresh token
        var refreshToken = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            _logger.LogWarning("Intento de refresh con token inválido o expirado");
            throw new UnauthorizedAccessException("Refresh token inválido o expirado");
        }

        // Obtener usuario
        var user = await _authRepository.GetUserByIdAsync(refreshToken.UserId, cancellationToken);
        if (user == null || !user.IsActive || user.IsDeleted)
        {
            _logger.LogWarning("Intento de refresh para usuario inactivo o eliminado: {UserId}", refreshToken.UserId);
            throw new UnauthorizedAccessException("Usuario inactivo o eliminado");
        }

        // Revocar el refresh token antiguo
        await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken: cancellationToken);

        // Generar nuevos tokens
        var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user, cancellationToken);
        var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken: cancellationToken);

        // Guardar nuevo refresh token
        await _authRepository.SaveRefreshTokenAsync(newRefreshToken, cancellationToken);

        _logger.LogInformation("Tokens refrescados exitosamente para usuario: {UserId}", user.Id);

        // Retornar respuesta
        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresIn = 900 // 15 minutos
        };
    }
}


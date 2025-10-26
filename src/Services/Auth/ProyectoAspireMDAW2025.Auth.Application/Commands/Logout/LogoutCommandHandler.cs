using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Contracts.Events.Auth;
using ProyectoAspireMDAW2025.Messaging.Abstractions;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Logout;

/// <summary>
/// Handler para LogoutCommand
/// </summary>
public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly ITokenService _tokenService;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        ITokenService tokenService,
        IEventPublisher eventPublisher,
        ILogger<LogoutCommandHandler> logger)
    {
        _tokenService = tokenService;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando logout para usuario: {UserId}", request.UserId);

        if (!string.IsNullOrEmpty(request.RefreshToken))
        {
            // Revocar refresh token específico
            await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken: cancellationToken);
            _logger.LogInformation("Refresh token específico revocado para usuario: {UserId}", request.UserId);
        }
        else
        {
            // Revocar todos los refresh tokens del usuario
            await _tokenService.RevokeAllUserTokensAsync(request.UserId, cancellationToken: cancellationToken);
            _logger.LogInformation("Todos los refresh tokens revocados para usuario: {UserId}", request.UserId);
        }

        // Publicar evento de token revocado
        var tokenRevokedEvent = new TokenRevokedEvent
        {
            UserId = request.UserId,
            TokenId = request.RefreshToken ?? "all-tokens",
            Reason = "User logout",
            TokenExpiresAt = DateTime.UtcNow.AddDays(7), // Refresh token expiration
            RevokedAt = DateTime.UtcNow
        };

        await _eventPublisher.PublishAsync(tokenRevokedEvent, cancellationToken);

        _logger.LogInformation("Logout exitoso para usuario: {UserId}", request.UserId);

        return true;
    }
}


using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Auth;
using ProyectoAspireMDAW2025.Contracts.Events.Auth;
using ProyectoAspireMDAW2025.Messaging.Abstractions;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Login;

/// <summary>
/// Handler para LoginCommand
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IAuthRepository authRepository,
        ITokenService tokenService,
        IEventPublisher eventPublisher,
        ILogger<LoginCommandHandler> logger)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando login para usuario: {Email}", request.Email);

        // Obtener usuario por email
        var user = await _authRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Intento de login con email no existente: {Email}", request.Email);
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Verificar si el usuario está activo
        if (!user.IsActive || user.IsDeleted)
        {
            _logger.LogWarning("Intento de login con usuario inactivo o eliminado: {UserId}", user.Id);
            throw new UnauthorizedAccessException("La cuenta está inactiva o eliminada");
        }

        // Validar credenciales
        var isValidPassword = await _authRepository.ValidateCredentialsAsync(user, request.Password, cancellationToken);
        if (!isValidPassword)
        {
            _logger.LogWarning("Intento de login con contraseña incorrecta: {Email}", request.Email);
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Actualizar último login
        user.UpdateLastLogin();

        // Generar tokens
        var accessToken = await _tokenService.GenerateAccessTokenAsync(user, cancellationToken);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken: cancellationToken);

        // Guardar refresh token
        await _authRepository.SaveRefreshTokenAsync(refreshToken, cancellationToken);

        _logger.LogInformation("Login exitoso para usuario: {UserId}", user.Id);

        // Publicar evento de login
        var userLoggedInEvent = new UserLoggedInEvent
        {
            UserId = user.Id,
            Email = user.Email!,
            LoggedInAt = DateTime.UtcNow
        };

        await _eventPublisher.PublishAsync(userLoggedInEvent, cancellationToken);

        // Retornar respuesta
        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = 900 // 15 minutos
        };
    }
}


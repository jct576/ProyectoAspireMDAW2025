using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Auth.Domain.Enums;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Auth;
using ProyectoAspireMDAW2025.Contracts.Events.Auth;
using ProyectoAspireMDAW2025.Messaging.Abstractions;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Register;

/// <summary>
/// Handler para RegisterCommand
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IAuthRepository authRepository,
        ITokenService tokenService,
        IEventPublisher eventPublisher,
        ILogger<RegisterCommandHandler> logger)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando registro de usuario: {Email}", request.Email);

        // Verificar si el usuario ya existe
        var existingUser = await _authRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            _logger.LogWarning("Intento de registro con email existente: {Email}", request.Email);
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Crear nuevo usuario
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false, // Requiere confirmación de email
            Status = UserStatus.PendingVerification,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Crear usuario con password
        var createdUser = await _authRepository.CreateUserAsync(user, request.Password, cancellationToken);

        _logger.LogInformation("Usuario creado exitosamente: {UserId}", createdUser.Id);

        // Generar tokens
        var accessToken = await _tokenService.GenerateAccessTokenAsync(createdUser, cancellationToken);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(createdUser.Id, cancellationToken: cancellationToken);

        // Guardar refresh token
        await _authRepository.SaveRefreshTokenAsync(refreshToken, cancellationToken);

        // Publicar evento de registro
        var userRegisteredEvent = new UserRegisteredEvent
        {
            UserId = createdUser.Id,
            Email = createdUser.Email!,
            RegisteredAt = createdUser.CreatedAt
        };

        await _eventPublisher.PublishAsync(userRegisteredEvent, cancellationToken);

        _logger.LogInformation("Evento UserRegisteredEvent publicado para usuario: {UserId}", createdUser.Id);

        // Retornar respuesta
        return new AuthResponse
        {
            UserId = createdUser.Id,
            Email = createdUser.Email!,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresIn = 900 // 15 minutos
        };
    }
}


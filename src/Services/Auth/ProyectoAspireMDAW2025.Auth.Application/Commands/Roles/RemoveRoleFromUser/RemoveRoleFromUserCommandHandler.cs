using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.RemoveRoleFromUser;

/// <summary>
/// Handler para RemoveRoleFromUserCommand
/// </summary>
public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, bool>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<RemoveRoleFromUserCommandHandler> _logger;

    public RemoveRoleFromUserCommandHandler(
        IAuthRepository authRepository,
        ILogger<RemoveRoleFromUserCommandHandler> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removiendo rol {RoleName} del usuario {UserId}", request.RoleName, request.UserId);

        // Verificar que el usuario existe
        var user = await _authRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Usuario no encontrado: {UserId}", request.UserId);
            throw new InvalidOperationException($"Usuario con ID {request.UserId} no encontrado");
        }

        // Verificar que el rol existe
        var roleExists = await _authRepository.RoleExistsAsync(request.RoleName, cancellationToken);
        if (!roleExists)
        {
            _logger.LogWarning("Rol no encontrado: {RoleName}", request.RoleName);
            throw new InvalidOperationException($"Rol '{request.RoleName}' no existe");
        }

        // Verificar si el usuario tiene el rol
        var isInRole = await _authRepository.IsInRoleAsync(user, request.RoleName, cancellationToken);
        if (!isInRole)
        {
            _logger.LogWarning("El usuario {UserId} no tiene el rol {RoleName}", request.UserId, request.RoleName);
            throw new InvalidOperationException($"El usuario no tiene el rol '{request.RoleName}'");
        }

        // Remover el rol
        var result = await _authRepository.RemoveRoleFromUserAsync(user, request.RoleName, cancellationToken);

        if (result)
        {
            _logger.LogInformation("Rol {RoleName} removido exitosamente del usuario {UserId}", request.RoleName, request.UserId);
        }

        return result;
    }
}


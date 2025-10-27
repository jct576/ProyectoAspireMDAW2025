using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.AssignRoleToUser;

/// <summary>
/// Handler para AssignRoleToUserCommand
/// </summary>
public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, bool>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<AssignRoleToUserCommandHandler> _logger;

    public AssignRoleToUserCommandHandler(
        IAuthRepository authRepository,
        ILogger<AssignRoleToUserCommandHandler> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Asignando rol {RoleName} al usuario {UserId}", request.RoleName, request.UserId);

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

        // Verificar si el usuario ya tiene el rol
        var isInRole = await _authRepository.IsInRoleAsync(user, request.RoleName, cancellationToken);
        if (isInRole)
        {
            _logger.LogWarning("El usuario {UserId} ya tiene el rol {RoleName}", request.UserId, request.RoleName);
            throw new InvalidOperationException($"El usuario ya tiene el rol '{request.RoleName}'");
        }

        // Asignar el rol
        var result = await _authRepository.AssignRoleToUserAsync(user, request.RoleName, cancellationToken);

        if (result)
        {
            _logger.LogInformation("Rol {RoleName} asignado exitosamente al usuario {UserId}", request.RoleName, request.UserId);
        }

        return result;
    }
}


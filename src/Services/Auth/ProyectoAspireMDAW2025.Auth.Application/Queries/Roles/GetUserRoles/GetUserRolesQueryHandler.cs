using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Roles.GetUserRoles;

/// <summary>
/// Handler para GetUserRolesQuery
/// </summary>
public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, UserRolesDto>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<GetUserRolesQueryHandler> _logger;

    public GetUserRolesQueryHandler(
        IAuthRepository authRepository,
        ILogger<GetUserRolesQueryHandler> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    public async Task<UserRolesDto> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo roles del usuario: {UserId}", request.UserId);

        // Verificar que el usuario existe
        var user = await _authRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Usuario no encontrado: {UserId}", request.UserId);
            throw new InvalidOperationException($"Usuario con ID {request.UserId} no encontrado");
        }

        // Obtener roles del usuario
        var roles = await _authRepository.GetUserRolesAsync(user, cancellationToken);

        // Obtener permisos efectivos del usuario
        var permissions = await _authRepository.GetUserPermissionsAsync(request.UserId, cancellationToken);

        _logger.LogInformation("Usuario {UserId} tiene {RoleCount} roles y {PermissionCount} permisos", 
            request.UserId, roles.Count, permissions.Count);

        return new UserRolesDto
        {
            UserId = user.Id,
            Email = user.Email!,
            Username = user.UserName,
            Roles = roles.ToList(),
            Permissions = permissions.ToList(),
            LastUpdated = DateTime.UtcNow
        };
    }
}


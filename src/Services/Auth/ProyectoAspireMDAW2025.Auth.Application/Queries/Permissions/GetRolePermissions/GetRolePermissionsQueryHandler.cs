using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Permissions.GetRolePermissions;

/// <summary>
/// Handler para GetRolePermissionsQuery
/// </summary>
public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, List<PermissionDto>>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<GetRolePermissionsQueryHandler> _logger;

    public GetRolePermissionsQueryHandler(
        IAuthRepository authRepository,
        ILogger<GetRolePermissionsQueryHandler> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    public async Task<List<PermissionDto>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo permisos del rol: {RoleName}", request.RoleName);

        // Verificar que el rol existe
        var role = await _authRepository.GetRoleByNameAsync(request.RoleName, cancellationToken);
        if (role == null)
        {
            _logger.LogWarning("Rol no encontrado: {RoleName}", request.RoleName);
            throw new InvalidOperationException($"Rol '{request.RoleName}' no encontrado");
        }

        // Obtener permisos del rol
        var permissions = await _authRepository.GetRolePermissionsAsync(role.Id, cancellationToken);

        // Mapear a DTOs
        var permissionDtos = permissions.Select(p => new PermissionDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Category = p.Category,
            CreatedAt = p.CreatedAt,
            IsAssigned = true // Todos están asignados porque vienen del rol
        }).ToList();

        _logger.LogInformation("Rol {RoleName} tiene {Count} permisos", request.RoleName, permissionDtos.Count);

        return permissionDtos;
    }
}


using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Permissions.GetAllPermissions;

/// <summary>
/// Handler para GetAllPermissionsQuery
/// </summary>
public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<PermissionDto>>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<GetAllPermissionsQueryHandler> _logger;

    public GetAllPermissionsQueryHandler(
        IAuthRepository authRepository,
        ILogger<GetAllPermissionsQueryHandler> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    public async Task<List<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo todos los permisos del sistema");

        // Obtener todos los permisos
        var permissions = await _authRepository.GetAllPermissionsAsync(cancellationToken);

        // Mapear a DTOs
        var permissionDtos = permissions.Select(p => new PermissionDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Category = p.Category,
            CreatedAt = p.CreatedAt,
            IsAssigned = false // Por defecto false, se usa en contexto de roles
        }).ToList();

        _logger.LogInformation("Se obtuvieron {Count} permisos", permissionDtos.Count);

        return permissionDtos;
    }
}


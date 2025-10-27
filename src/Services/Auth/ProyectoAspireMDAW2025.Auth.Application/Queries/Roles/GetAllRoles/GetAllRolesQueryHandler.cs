using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Roles.GetAllRoles;

/// <summary>
/// Handler para GetAllRolesQuery
/// </summary>
public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly IAuthRepository _authRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<GetAllRolesQueryHandler> _logger;

    public GetAllRolesQueryHandler(
        IAuthRepository authRepository,
        UserManager<ApplicationUser> userManager,
        ILogger<GetAllRolesQueryHandler> logger)
    {
        _authRepository = authRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo todos los roles del sistema");

        // Obtener todos los roles
        var roles = await _authRepository.GetAllRolesAsync(cancellationToken);

        // Mapear a DTOs
        var roleDtos = new List<RoleDto>();

        foreach (var role in roles)
        {
            // Obtener permisos del rol
            var permissions = await _authRepository.GetRolePermissionsAsync(role.Id, cancellationToken);

            // Contar usuarios con este rol
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

            roleDtos.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                NormalizedName = role.NormalizedName,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                UpdatedAt = null, // TODO: Agregar UpdatedAt a ApplicationRole si es necesario
                UserCount = usersInRole.Count,
                Permissions = permissions.Select(p => p.Name).ToList()
            });
        }

        _logger.LogInformation("Se obtuvieron {Count} roles", roleDtos.Count);

        return roleDtos;
    }
}


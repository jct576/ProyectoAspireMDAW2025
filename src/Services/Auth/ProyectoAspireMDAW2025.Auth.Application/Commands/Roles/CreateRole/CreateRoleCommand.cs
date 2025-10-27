using MediatR;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.CreateRole;

/// <summary>
/// Command para crear un nuevo rol
/// </summary>
public record CreateRoleCommand(
    string Name,
    string? Description = null,
    List<string>? Permissions = null
) : IRequest<RoleDto>;


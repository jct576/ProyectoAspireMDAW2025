using MediatR;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Permissions.GetRolePermissions;

/// <summary>
/// Query para obtener los permisos de un rol específico
/// </summary>
public record GetRolePermissionsQuery(string RoleName) : IRequest<List<PermissionDto>>;


using MediatR;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Permissions.GetAllPermissions;

/// <summary>
/// Query para obtener todos los permisos del sistema
/// </summary>
public record GetAllPermissionsQuery() : IRequest<List<PermissionDto>>;


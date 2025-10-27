using MediatR;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Roles.GetAllRoles;

/// <summary>
/// Query para obtener todos los roles del sistema
/// </summary>
public record GetAllRolesQuery() : IRequest<List<RoleDto>>;


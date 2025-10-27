using MediatR;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Roles.GetUserRoles;

/// <summary>
/// Query para obtener los roles de un usuario específico
/// </summary>
public record GetUserRolesQuery(Guid UserId) : IRequest<UserRolesDto>;


using MediatR;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.AssignRoleToUser;

/// <summary>
/// Command para asignar un rol a un usuario
/// </summary>
public record AssignRoleToUserCommand(
    Guid UserId,
    string RoleName
) : IRequest<bool>;


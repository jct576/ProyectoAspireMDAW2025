using MediatR;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.RemoveRoleFromUser;

/// <summary>
/// Command para remover un rol de un usuario
/// </summary>
public record RemoveRoleFromUserCommand(
    Guid UserId,
    string RoleName
) : IRequest<bool>;


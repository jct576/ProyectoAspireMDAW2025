using MediatR;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Logout;

/// <summary>
/// Command para logout (revocar tokens)
/// </summary>
public record LogoutCommand(
    Guid UserId,
    string? RefreshToken = null
) : IRequest<bool>;


using FluentValidation;
using ProyectoAspireMDAW2025.Common.Constants;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.RemoveRoleFromUser;

/// <summary>
/// Validador para RemoveRoleFromUserCommand
/// </summary>
public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
{
    public RemoveRoleFromUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El ID del usuario es requerido");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("El nombre del rol es requerido")
            .MaximumLength(50).WithMessage("El nombre del rol no puede exceder 50 caracteres")
            .Must(BeAValidRole).WithMessage("El rol especificado no es válido. Roles válidos: Admin, Manager, User, Guest");
    }

    private bool BeAValidRole(string roleName)
    {
        return Common.Constants.Roles.All.Contains(roleName, StringComparer.OrdinalIgnoreCase);
    }
}


using FluentValidation;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.CreateRole;

/// <summary>
/// Validador para CreateRoleCommand
/// </summary>
public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del rol es requerido")
            .MaximumLength(50).WithMessage("El nombre del rol no puede exceder 50 caracteres")
            .Matches(@"^[a-zA-Z][a-zA-Z0-9_]*$")
            .WithMessage("El nombre del rol debe comenzar con una letra y solo puede contener letras, números y guiones bajos");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleForEach(x => x.Permissions)
            .NotEmpty().WithMessage("Los permisos no pueden estar vacíos")
            .When(x => x.Permissions != null && x.Permissions.Any());
    }
}


using FluentValidation;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Permissions.GetRolePermissions;

/// <summary>
/// Validador para GetRolePermissionsQuery
/// </summary>
public class GetRolePermissionsQueryValidator : AbstractValidator<GetRolePermissionsQuery>
{
    public GetRolePermissionsQueryValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("El nombre del rol es requerido")
            .MaximumLength(50).WithMessage("El nombre del rol no puede exceder 50 caracteres");
    }
}


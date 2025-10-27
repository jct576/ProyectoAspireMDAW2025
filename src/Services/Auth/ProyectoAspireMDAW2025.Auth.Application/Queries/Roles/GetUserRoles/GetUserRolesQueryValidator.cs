using FluentValidation;

namespace ProyectoAspireMDAW2025.Auth.Application.Queries.Roles.GetUserRoles;

/// <summary>
/// Validador para GetUserRolesQuery
/// </summary>
public class GetUserRolesQueryValidator : AbstractValidator<GetUserRolesQuery>
{
    public GetUserRolesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El ID del usuario es requerido");
    }
}


using FluentValidation;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Register;

/// <summary>
/// Validador para RegisterCommand
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email no es válido")
            .MaximumLength(256).WithMessage("El email no puede exceder 256 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres")
            .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula")
            .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una minúscula")
            .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número")
            .Matches(@"[\W_]").WithMessage("La contraseña debe contener al menos un carácter especial");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("La confirmación de contraseña es requerida")
            .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");
    }
}


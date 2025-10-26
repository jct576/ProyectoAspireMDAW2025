using FluentValidation;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Login;

/// <summary>
/// Validador para LoginCommand
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email no es válido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida");
    }
}


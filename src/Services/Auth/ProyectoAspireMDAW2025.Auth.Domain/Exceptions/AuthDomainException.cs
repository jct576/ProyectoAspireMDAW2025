namespace ProyectoAspireMDAW2025.Auth.Domain.Exceptions;

/// <summary>
/// Excepción de dominio para Auth Service
/// </summary>
public class AuthDomainException : Exception
{
    public AuthDomainException() : base() { }
    
    public AuthDomainException(string message) : base(message) { }
    
    public AuthDomainException(string message, Exception innerException) 
        : base(message, innerException) { }
}


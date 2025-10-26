using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAspireMDAW2025.Auth.Application.Commands.Login;
using ProyectoAspireMDAW2025.Auth.Application.Commands.Logout;
using ProyectoAspireMDAW2025.Auth.Application.Commands.RefreshToken;
using ProyectoAspireMDAW2025.Auth.Application.Commands.Register;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Auth;

namespace ProyectoAspireMDAW2025.Auth.Api.Controllers;

/// <summary>
/// Controlador de autenticación - Endpoints para registro, login, refresh token y logout
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Registrar un nuevo usuario
    /// </summary>
    /// <param name="command">Datos de registro (email, password, confirmPassword)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>AuthResponse con tokens JWT</returns>
    /// <response code="200">Usuario registrado exitosamente</response>
    /// <response code="400">Datos de registro inválidos</response>
    /// <response code="409">El email ya está registrado</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponse>> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Register request received for email: {Email}", command.Email);

            var response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("User registered successfully: {Email}", command.Email);

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed for email: {Email}", command.Email);
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", command.Email);
            return BadRequest(new { message = "Registration failed. Please try again." });
        }
    }

    /// <summary>
    /// Iniciar sesión con email y password
    /// </summary>
    /// <param name="command">Credenciales de login (email, password)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>AuthResponse con tokens JWT</returns>
    /// <response code="200">Login exitoso</response>
    /// <response code="400">Datos de login inválidos</response>
    /// <response code="401">Credenciales incorrectas</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Login request received for email: {Email}", command.Email);

            var response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("User logged in successfully: {Email}", command.Email);

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Login failed for email: {Email}", command.Email);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", command.Email);
            return BadRequest(new { message = "Login failed. Please try again." });
        }
    }

    /// <summary>
    /// Renovar access token usando refresh token
    /// </summary>
    /// <param name="command">Refresh token</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>AuthResponse con nuevos tokens JWT</returns>
    /// <response code="200">Token renovado exitosamente</response>
    /// <response code="400">Refresh token inválido</response>
    /// <response code="401">Refresh token expirado o revocado</response>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> RefreshToken(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Refresh token request received");

            var response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Token refreshed successfully");

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Refresh token failed");
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return BadRequest(new { message = "Token refresh failed. Please try again." });
        }
    }

    /// <summary>
    /// Cerrar sesión (revocar refresh tokens)
    /// </summary>
    /// <param name="command">UserId y opcionalmente RefreshToken específico</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Confirmación de logout</returns>
    /// <response code="200">Logout exitoso</response>
    /// <response code="400">Error al cerrar sesión</response>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Logout(
        [FromBody] LogoutCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Logout request received for user: {UserId}", command.UserId);

            var result = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("User logged out successfully: {UserId}", command.UserId);

            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user: {UserId}", command.UserId);
            return BadRequest(new { message = "Logout failed. Please try again." });
        }
    }
}


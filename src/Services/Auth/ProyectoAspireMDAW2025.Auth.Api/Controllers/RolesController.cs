using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAspireMDAW2025.Auth.Api.Attributes;
using ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.AssignRoleToUser;
using ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.CreateRole;
using ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.RemoveRoleFromUser;
using ProyectoAspireMDAW2025.Auth.Application.Queries.Roles.GetAllRoles;
using ProyectoAspireMDAW2025.Auth.Application.Queries.Roles.GetUserRoles;
using ProyectoAspireMDAW2025.Common.Constants;
using ProyectoAspireMDAW2025.Common.DTOs.Requests.Roles;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Api.Controllers;

/// <summary>
/// Controlador de gestión de roles
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize] // Todos los endpoints requieren autenticación
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IMediator mediator, ILogger<RolesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todos los roles del sistema
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de roles con sus permisos</returns>
    /// <response code="200">Lista de roles obtenida exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para ver roles</response>
    [HttpGet]
    [RequirePermission(Permissions.Roles.Read)]
    [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<RoleDto>>> GetAllRoles(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("GetAllRoles request received");

            var query = new GetAllRolesQuery();
            var roles = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Retrieved {Count} roles", roles.Count);

            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all roles");
            return StatusCode(500, new { message = "Error retrieving roles. Please try again." });
        }
    }

    /// <summary>
    /// Obtener roles de un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Roles y permisos del usuario</returns>
    /// <response code="200">Roles del usuario obtenidos exitosamente</response>
    /// <response code="400">UserId inválido</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para ver roles de usuarios</response>
    /// <response code="404">Usuario no encontrado</response>
    [HttpGet("user/{userId:guid}")]
    [RequirePermission(Permissions.Users.Read, Permissions.Roles.Read)]
    [ProducesResponseType(typeof(UserRolesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserRolesDto>> GetUserRoles(
        Guid userId,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("GetUserRoles request received for user: {UserId}", userId);

            var query = new GetUserRolesQuery(userId);
            var userRoles = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Retrieved roles for user {UserId}", userId);

            return Ok(userRoles);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "User not found: {UserId}", userId);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles for user {UserId}", userId);
            return StatusCode(500, new { message = "Error retrieving user roles. Please try again." });
        }
    }

    /// <summary>
    /// Crear un nuevo rol
    /// </summary>
    /// <param name="request">Datos del nuevo rol</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Rol creado</returns>
    /// <response code="200">Rol creado exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para gestionar roles</response>
    /// <response code="409">El rol ya existe</response>
    [HttpPost]
    [RequirePermission(Permissions.Roles.Manage)]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RoleDto>> CreateRole(
        [FromBody] CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("CreateRole request received for role: {RoleName}", request.Name);

            var command = new CreateRoleCommand(request.Name, request.Description, request.Permissions);
            var role = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Role created successfully: {RoleName}", request.Name);

            return Ok(role);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Role creation failed: {RoleName}", request.Name);
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role: {RoleName}", request.Name);
            return BadRequest(new { message = "Role creation failed. Please try again." });
        }
    }

    /// <summary>
    /// Asignar un rol a un usuario
    /// </summary>
    /// <param name="request">Datos de asignación (UserId, RoleName)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Confirmación de asignación</returns>
    /// <response code="200">Rol asignado exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para asignar roles</response>
    /// <response code="404">Usuario o rol no encontrado</response>
    [HttpPost("assign")]
    [RequirePermission(Permissions.Roles.Assign)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AssignRole(
        [FromBody] AssignRoleRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("AssignRole request received: User {UserId}, Role {RoleName}", 
                request.UserId, request.RoleName);

            var command = new AssignRoleToUserCommand(request.UserId, request.RoleName);
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Role {RoleName} assigned to user {UserId}", 
                request.RoleName, request.UserId);

            return Ok(new { message = $"Role '{request.RoleName}' assigned successfully" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Role assignment failed: User {UserId}, Role {RoleName}", 
                request.UserId, request.RoleName);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning role: User {UserId}, Role {RoleName}", 
                request.UserId, request.RoleName);
            return BadRequest(new { message = "Role assignment failed. Please try again." });
        }
    }

    /// <summary>
    /// Remover un rol de un usuario
    /// </summary>
    /// <param name="request">Datos de remoción (UserId, RoleName)</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Confirmación de remoción</returns>
    /// <response code="200">Rol removido exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para asignar roles</response>
    /// <response code="404">Usuario o rol no encontrado</response>
    [HttpPost("remove")]
    [RequirePermission(Permissions.Roles.Assign)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveRole(
        [FromBody] RemoveRoleRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("RemoveRole request received: User {UserId}, Role {RoleName}", 
                request.UserId, request.RoleName);

            var command = new RemoveRoleFromUserCommand(request.UserId, request.RoleName);
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Role {RoleName} removed from user {UserId}", 
                request.RoleName, request.UserId);

            return Ok(new { message = $"Role '{request.RoleName}' removed successfully" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Role removal failed: User {UserId}, Role {RoleName}", 
                request.UserId, request.RoleName);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing role: User {UserId}, Role {RoleName}", 
                request.UserId, request.RoleName);
            return BadRequest(new { message = "Role removal failed. Please try again." });
        }
    }
}


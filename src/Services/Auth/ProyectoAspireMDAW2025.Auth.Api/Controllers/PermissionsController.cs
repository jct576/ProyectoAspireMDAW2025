using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAspireMDAW2025.Auth.Api.Attributes;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Auth.Application.Queries.Permissions.GetAllPermissions;
using ProyectoAspireMDAW2025.Auth.Application.Queries.Permissions.GetRolePermissions;
using ProyectoAspireMDAW2025.Common.Constants;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Api.Controllers;

/// <summary>
/// Controlador de gestión de permisos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize] // Todos los endpoints requieren autenticación
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(
        IMediator mediator,
        IPermissionService permissionService,
        ILogger<PermissionsController> logger)
    {
        _mediator = mediator;
        _permissionService = permissionService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todos los permisos del sistema
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de todos los permisos disponibles</returns>
    /// <response code="200">Lista de permisos obtenida exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para ver permisos</response>
    [HttpGet]
    [RequirePermission(Permissions.PermissionsManagement.Read)]
    [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<PermissionDto>>> GetAllPermissions(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("GetAllPermissions request received");

            var query = new GetAllPermissionsQuery();
            var permissions = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Retrieved {Count} permissions", permissions.Count);

            return Ok(permissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all permissions");
            return StatusCode(500, new { message = "Error retrieving permissions. Please try again." });
        }
    }

    /// <summary>
    /// Obtener permisos de un rol específico
    /// </summary>
    /// <param name="roleName">Nombre del rol</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de permisos del rol</returns>
    /// <response code="200">Permisos del rol obtenidos exitosamente</response>
    /// <response code="400">Nombre de rol inválido</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para ver permisos</response>
    /// <response code="404">Rol no encontrado</response>
    [HttpGet("role/{roleName}")]
    [RequirePermission(Permissions.PermissionsManagement.Read, Permissions.Roles.Read)]
    [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<PermissionDto>>> GetRolePermissions(
        string roleName,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("GetRolePermissions request received for role: {RoleName}", roleName);

            var query = new GetRolePermissionsQuery(roleName);
            var permissions = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Retrieved {Count} permissions for role {RoleName}", 
                permissions.Count, roleName);

            return Ok(permissions);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Role not found: {RoleName}", roleName);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting permissions for role {RoleName}", roleName);
            return StatusCode(500, new { message = "Error retrieving role permissions. Please try again." });
        }
    }

    /// <summary>
    /// Asignar un permiso a un rol
    /// </summary>
    /// <param name="roleId">ID del rol</param>
    /// <param name="permissionName">Nombre del permiso</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Confirmación de asignación</returns>
    /// <response code="200">Permiso asignado exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para gestionar permisos</response>
    /// <response code="404">Rol o permiso no encontrado</response>
    [HttpPost("assign")]
    [RequirePermission(Permissions.PermissionsManagement.Manage)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AssignPermission(
        [FromQuery] Guid roleId,
        [FromQuery] string permissionName,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("AssignPermission request received: Role {RoleId}, Permission {PermissionName}", 
                roleId, permissionName);

            var result = await _permissionService.AssignPermissionToRoleAsync(roleId, permissionName, cancellationToken);

            if (result)
            {
                _logger.LogInformation("Permission {PermissionName} assigned to role {RoleId}", 
                    permissionName, roleId);
                return Ok(new { message = $"Permission '{permissionName}' assigned successfully" });
            }
            else
            {
                _logger.LogWarning("Permission {PermissionName} already assigned to role {RoleId}", 
                    permissionName, roleId);
                return BadRequest(new { message = "Permission already assigned to role" });
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Permission assignment failed: Role {RoleId}, Permission {PermissionName}", 
                roleId, permissionName);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning permission: Role {RoleId}, Permission {PermissionName}", 
                roleId, permissionName);
            return BadRequest(new { message = "Permission assignment failed. Please try again." });
        }
    }

    /// <summary>
    /// Remover un permiso de un rol
    /// </summary>
    /// <param name="roleId">ID del rol</param>
    /// <param name="permissionName">Nombre del permiso</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Confirmación de remoción</returns>
    /// <response code="200">Permiso removido exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para gestionar permisos</response>
    /// <response code="404">Rol o permiso no encontrado</response>
    [HttpPost("remove")]
    [RequirePermission(Permissions.PermissionsManagement.Manage)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemovePermission(
        [FromQuery] Guid roleId,
        [FromQuery] string permissionName,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("RemovePermission request received: Role {RoleId}, Permission {PermissionName}", 
                roleId, permissionName);

            var result = await _permissionService.RemovePermissionFromRoleAsync(roleId, permissionName, cancellationToken);

            if (result)
            {
                _logger.LogInformation("Permission {PermissionName} removed from role {RoleId}", 
                    permissionName, roleId);
                return Ok(new { message = $"Permission '{permissionName}' removed successfully" });
            }
            else
            {
                _logger.LogWarning("Permission {PermissionName} not assigned to role {RoleId}", 
                    permissionName, roleId);
                return BadRequest(new { message = "Permission not assigned to role" });
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Permission removal failed: Role {RoleId}, Permission {PermissionName}", 
                roleId, permissionName);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing permission: Role {RoleId}, Permission {PermissionName}", 
                roleId, permissionName);
            return BadRequest(new { message = "Permission removal failed. Please try again." });
        }
    }

    /// <summary>
    /// Sincronizar permisos del sistema (crear permisos faltantes desde constantes)
    /// Solo para administradores
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Confirmación de sincronización</returns>
    /// <response code="200">Permisos sincronizados exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="403">No tiene permiso para gestionar permisos</response>
    [HttpPost("sync")]
    [RequirePermission(Permissions.PermissionsManagement.Manage)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> SyncPermissions(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("SyncPermissions request received");

            await _permissionService.SyncPermissionsAsync(cancellationToken);

            _logger.LogInformation("Permissions synchronized successfully");

            return Ok(new { message = "Permissions synchronized successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error synchronizing permissions");
            return StatusCode(500, new { message = "Permission synchronization failed. Please try again." });
        }
    }
}


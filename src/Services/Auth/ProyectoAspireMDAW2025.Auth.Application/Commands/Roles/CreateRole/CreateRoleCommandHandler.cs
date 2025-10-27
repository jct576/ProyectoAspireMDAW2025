using MediatR;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Common.DTOs.Responses.Roles;

namespace ProyectoAspireMDAW2025.Auth.Application.Commands.Roles.CreateRole;

/// <summary>
/// Handler para CreateRoleCommand
/// </summary>
public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<CreateRoleCommandHandler> _logger;

    public CreateRoleCommandHandler(
        IAuthRepository authRepository,
        ILogger<CreateRoleCommandHandler> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creando nuevo rol: {RoleName}", request.Name);

        // Verificar si el rol ya existe
        var existingRole = await _authRepository.GetRoleByNameAsync(request.Name, cancellationToken);
        if (existingRole != null)
        {
            _logger.LogWarning("Intento de crear rol existente: {RoleName}", request.Name);
            throw new InvalidOperationException($"El rol '{request.Name}' ya existe");
        }

        // Crear el rol
        var role = await _authRepository.CreateRoleAsync(request.Name, request.Description, cancellationToken);

        _logger.LogInformation("Rol creado exitosamente: {RoleId} - {RoleName}", role.Id, request.Name);

        // TODO: En PARTE 5 agregar lógica para asignar permisos al rol si se especificaron

        // Retornar DTO
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name!,
            NormalizedName = role.NormalizedName,
            Description = role.Description,
            CreatedAt = role.CreatedAt,
            UpdatedAt = null,
            UserCount = 0,
            Permissions = request.Permissions ?? new List<string>()
        };
    }
}


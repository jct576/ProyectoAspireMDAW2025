using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Data;
using ProyectoAspireMDAW2025.Common.Constants;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Services;

/// <summary>
/// Servicio para generación y validación de tokens JWT y Refresh Tokens
/// </summary>
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthDbContext _context;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<TokenService> _logger;

    public TokenService(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        AuthDbContext context,
        IPermissionService permissionService,
        ILogger<TokenService> logger)
    {
        _configuration = configuration;
        _userManager = userManager;
        _context = context;
        _permissionService = permissionService;
        _logger = logger;
    }

    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            // Obtener roles del usuario
            var roles = await _userManager.GetRolesAsync(user);

            // Obtener permisos del usuario
            var permissions = await _permissionService.GetUserPermissionsAsync(user.Id, cancellationToken);

            // Crear claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Agregar username si existe
            if (!string.IsNullOrEmpty(user.UserName))
            {
                claims.Add(new Claim(CustomClaimTypes.Username, user.UserName));
            }

            // Agregar UserId como custom claim
            claims.Add(new Claim(CustomClaimTypes.UserId, user.Id.ToString()));

            // Agregar roles como claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Agregar permisos como claim JSON array
            if (permissions.Any())
            {
                var permissionsJson = JsonSerializer.Serialize(permissions);
                claims.Add(new Claim(CustomClaimTypes.Permissions, permissionsJson));

                _logger.LogInformation("Added {PermissionCount} permissions to JWT for user {UserId}",
                    permissions.Count, user.Id);
            }

            // Configuración JWT
            var jwtSecret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "https://localhost:5000";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "https://localhost:5000";
            var jwtExpirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "15");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtExpirationMinutes),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Access token generated for user {UserId}", user.Id);

            return tokenString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating access token for user {UserId}", user.Id);
            throw;
        }
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // Generar token criptográficamente seguro
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var tokenString = Convert.ToBase64String(randomBytes);

            // Configuración de expiración
            var refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = tokenString,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                IsRevoked = false
            };

            _logger.LogInformation("Refresh token generated for user {UserId}", userId);

            return await Task.FromResult(refreshToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating refresh token for user {UserId}", userId);
            throw;
        }
    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);

            if (refreshToken == null)
            {
                _logger.LogWarning("Refresh token not found: {Token}", token);
                return null;
            }

            if (!refreshToken.IsActive)
            {
                _logger.LogWarning("Refresh token is not active: {TokenId}", refreshToken.Id);
                return null;
            }

            _logger.LogInformation("Refresh token validated: {TokenId}", refreshToken.Id);

            return refreshToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating refresh token");
            throw;
        }
    }

    public async Task RevokeRefreshTokenAsync(string token, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);

            if (refreshToken == null)
            {
                _logger.LogWarning("Refresh token not found for revocation: {Token}", token);
                return;
            }

            refreshToken.Revoke(ipAddress);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Refresh token revoked: {TokenId}", refreshToken.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking refresh token");
            throw;
        }
    }

    public async Task RevokeAllUserTokensAsync(Guid userId, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (var token in activeTokens)
            {
                token.Revoke(ipAddress);
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("All refresh tokens revoked for user {UserId}. Count: {Count}", userId, activeTokens.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking all tokens for user {UserId}", userId);
            throw;
        }
    }
}


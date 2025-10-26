using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProyectoAspireMDAW2025.Auth.Application.Interfaces;
using ProyectoAspireMDAW2025.Auth.Domain.Entities;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Data;

namespace ProyectoAspireMDAW2025.Auth.Infrastructure.Repositories;

/// <summary>
/// Repositorio para operaciones de autenticación y gestión de usuarios
/// </summary>
public class AuthRepository : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthDbContext _context;
    private readonly ILogger<AuthRepository> _logger;

    public AuthRepository(
        UserManager<ApplicationUser> userManager,
        AuthDbContext context,
        ILogger<AuthRepository> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _userManager.FindByEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by email: {Email}", email);
            throw;
        }
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user {Email}. Errors: {Errors}", user.Email, errors);
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            _logger.LogInformation("User created successfully: {UserId}", user.Id);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", user.Email);
            throw;
        }
    }

    public async Task<bool> ValidateCredentialsAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var isValid = await _userManager.CheckPasswordAsync(user, password);

            if (isValid)
            {
                _logger.LogInformation("Credentials validated for user: {UserId}", user.Id);
            }
            else
            {
                _logger.LogWarning("Invalid credentials for user: {UserId}", user.Id);
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating credentials for user: {UserId}", user.Id);
            throw;
        }
    }

    public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation("Retrieved {Count} roles for user: {UserId}", roles.Count, user.Id);

            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles for user: {UserId}", user.Id);
            throw;
        }
    }

    public async Task SaveRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Refresh token saved: {TokenId} for user: {UserId}", refreshToken.Id, refreshToken.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving refresh token for user: {UserId}", refreshToken.UserId);
            throw;
        }
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);

            if (refreshToken != null)
            {
                _logger.LogInformation("Refresh token found: {TokenId}", refreshToken.Id);
            }
            else
            {
                _logger.LogWarning("Refresh token not found: {Token}", token);
            }

            return refreshToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting refresh token");
            throw;
        }
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Refresh token updated: {TokenId}", refreshToken.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating refresh token: {TokenId}", refreshToken.Id);
            throw;
        }
    }

    public async Task<IList<RefreshToken>> GetUserActiveRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} active refresh tokens for user: {UserId}", activeTokens.Count, userId);

            return activeTokens;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active refresh tokens for user: {UserId}", userId);
            throw;
        }
    }
}


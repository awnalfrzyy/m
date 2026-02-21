using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace diggie_server.src.infrastructure.auth.jwt;

public class JwtStrategy
{
    private readonly IConfiguration _config;
    private readonly ILogger<JwtStrategy>? _logger;

    public JwtStrategy(IConfiguration config, ILogger<JwtStrategy>? logger = null)
    {
        _config = config;
        _logger = logger;
    }

    public (bool Valid, ClaimsPrincipal? Principal) ValidateToken(string token)
    {
        _logger?.LogDebug("Validating JWT");
        if (string.IsNullOrWhiteSpace(token))
        {
            _logger?.LogDebug("No token provided to ValidateToken");
            return (false, null);
        }

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token.Substring(7).Trim();

        if (token.Split('.').Length != 3)
        {
            _logger?.LogWarning("JWT token malformed (expected 3 segments)");
            return (false, null);
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["JWT_SECRET"]!);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _config["JWT_ISSUER"],
            ValidateAudience = true,
            ValidAudience = _config["JWT_AUDIENCE"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            _logger?.LogInformation("JWT validated successfully");
            return (true, principal);
        }
        catch (SecurityTokenMalformedException ex)
        {
            _logger?.LogWarning(ex, "JWT validation failed: malformed token");
            return (false, null);
        }
        catch (SecurityTokenException ex)
        {
            _logger?.LogWarning(ex, "JWT validation failed");
            return (false, null);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error during JWT validation");
            return (false, null);
        }
    }
}

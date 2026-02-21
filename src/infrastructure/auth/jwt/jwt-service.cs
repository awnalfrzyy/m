using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using diggie_server.src.infrastructure.persistence.entities;

namespace diggie_server.src.infrastructure.auth.jwt;

public class JwtService
{
    private readonly IConfiguration _config;
    private readonly ILogger<JwtService>? _logger;

    public JwtService(IConfiguration config, ILogger<JwtService>? logger = null)
    {
        _config = config;
        _logger = logger;
    }

    public string Generate(EntityUser user)
    {
        _logger?.LogDebug("Generating JWT for user {UserId}", user.Id);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JWT_SECRET"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JWT_ISSUER"],
            audience: _config["JWT_AUDIENCE"],
            expires: DateTime.UtcNow.AddMinutes(
            int.TryParse(_config["JWT_EXPIRE_MINUTES"], out var minutes) ? minutes : 60
            ),
            claims: claims,
            signingCredentials: creds
        );


        var written = new JwtSecurityTokenHandler().WriteToken(token);
        _logger?.LogInformation("JWT generated for user {UserId}", user.Id);
        return written;
    }
}
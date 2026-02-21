namespace diggie_server.src.infrastructure.auth.guard;

public class JwtGuardMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtGuardMiddleware>? _logger;

    public JwtGuardMiddleware(RequestDelegate next, ILogger<JwtGuardMiddleware>? logger = null)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, diggie_server.src.infrastructure.auth.jwt.JwtStrategy strategy)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }

        string? token = null;

        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer "))
        {
            token = authHeader.Substring("Bearer ".Length).Trim();
        }

        if (string.IsNullOrEmpty(token))
        {
            token = context.Request.Cookies["X-Access-Token"];
        }

        if (string.IsNullOrEmpty(token))
        {
            _logger?.LogWarning("Access attempt without token (Header or Cookie missing)");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = "Opps! Kamu butuh login dulu." });
            return;
        }

        var (valid, principal) = strategy.ValidateToken(token);
        if (!valid || principal == null)
        {
            _logger?.LogWarning("Invalid token provided");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = "Token sudah tidak berlaku." });
            return;
        }

        context.User = principal;
        await _next(context);
    }
}

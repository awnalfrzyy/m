using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using diggie_server.src.identity.features.register;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterHandler _registerHandler;
    public AuthController(RegisterHandler registerHandler) => _registerHandler = registerHandler;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _registerHandler.Handle(request);
        return Ok(response);
    }
}
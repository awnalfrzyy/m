using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using diggie_server.src.identity.features.register;
using diggie_server.src.identity.features.login;
using Microsoft.AspNetCore.Authorization;
using diggie_server.src.identity.features.otp.send;
using diggie_server.src.identity.features.otp;
using diggie_server.src.finance.features.receipt.send;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly RegisterHandler _registerHandler;
    private readonly SendOTPHandler _sendOTPHandler;
    private readonly LoginHandler _loginHandler;
    private readonly IEmailService _emailService;

    public AuthController(RegisterHandler registerHandler, SendOTPHandler sendOTPHandler, LoginHandler loginHandler, IEmailService emailService)
    {
        _registerHandler = registerHandler;
        _sendOTPHandler = sendOTPHandler;
        _loginHandler = loginHandler;
        _emailService = emailService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _registerHandler.Handle(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
     [FromBody] LoginRequest request,
     [FromServices] LoginHandler handler)
    {
        var result = await handler.Handle(request);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(2)
        };

        Response.Cookies.Append("X-Access-Token", result.token, cookieOptions);

        return Ok(result.response);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("X-Access-Token");
        return Ok(new { message = "Logged out successfully" });
    }

    [AllowAnonymous]
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendOTPRequest request)
    {
        var result = await _sendOTPHandler.Handle(request);
        return Ok(new { success = result });
    }

    [AllowAnonymous]
    [HttpPost("send-struk")]
    public async Task<IActionResult> SendStruk([FromBody] SendStrukRequest request)
    {
        var handler = new SendStrukHandler(_emailService);

        var result = await handler.Handle(request);

        return Ok(new
        {
            success = result,
            message = result ? "Cek Mailtrap, Win!" : "Waduh gagal, cek log terminal!"
        });
    }
}
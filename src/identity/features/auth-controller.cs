using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using diggie_server.src.identity.features.register;
using diggie_server.src.identity.features.login;
using Microsoft.AspNetCore.Authorization;
using diggie_server.src.identity.features.otp.send;
using diggie_server.src.identity.features.reset;
using diggie_server.src.identity.features.otp.verify;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly RegisterHandler registerHandler;
    private readonly SendOTPHandler sendOTPHandler;
    private readonly LoginHandler loginHandler;
    private readonly ResetHandler resetHandler;
    private readonly IEmailService emailService;
    private readonly VerifyOtpHandler verifyOtpHandler;

    public AuthController(
        RegisterHandler registerHandler,
        SendOTPHandler sendOTPHandler,
        LoginHandler loginHandler,
        ResetHandler resetHandler,
        IEmailService emailService,
        VerifyOtpHandler verifyOtpHandler

     )
    {
        this.registerHandler = registerHandler;
        this.sendOTPHandler = sendOTPHandler;
        this.loginHandler = loginHandler;
        this.resetHandler = resetHandler;
        this.emailService = emailService;
        this.verifyOtpHandler = verifyOtpHandler;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await registerHandler.Handle(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await loginHandler.Handle(request);

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
    [HttpPost("reset-password/request")]
    public async Task<IActionResult> RequestReset([FromBody] SendOTPRequest request)
    {
        var isSent = await sendOTPHandler.Handle(request);

        if (!isSent) return BadRequest(new { message = "Gagal mengirim OTP. Pastikan email terdaftar." });

        return Ok(new { message = "OTP sent to your email." });
    }

    [AllowAnonymous]
    [HttpPost("reset-password/verify")]
    public async Task<IActionResult> VerifyResetOtp([FromBody] VerifyOtpRequest request)
    {
        var isVerified = await verifyOtpHandler.Handle(request);

        if (!isVerified)
            return BadRequest(new { message = "OTP salah atau sudah kedaluwarsa." });

        return Ok(new { message = "OTP verified. Proceed to change password." });
    }

    [AllowAnonymous]
    [HttpPost("reset-password/submit")]
    public async Task<IActionResult> SubmitNewPassword([FromBody] ResetRequest request)
    {
        var result = await resetHandler.Handle(request);

        if (!result) return BadRequest(new { message = "Gagal reset password. Pastikan OTP sudah diverifikasi." });

        return Ok(new { success = true, message = "Password changed successfully." });
    }
}

// [AllowAnonymous]
// [HttpPost("send-struk")]
// public async Task<IActionResult> SendStruk([FromBody] SendStrukRequest request)
// {
//     var handler = new SendStrukHandler(emailService);

//     var result = await handler.Handle(request);

//     return Ok(new
//     {
//         success = result,
//         message = result ? "Cek Mailtrap, Win!" : "Waduh gagal, cek log terminal!"
//     });
// }
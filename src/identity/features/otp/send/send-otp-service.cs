using diggie_server.src.infrastructure.persistence.repositories;
using diggie_server.src.shared.email.models;

namespace diggie_server.src.identity.features.otp.send;

public class SendOTPHandler
{
    private readonly RepositoryOtp _repositoryOtp;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendOTPHandler> _logger;

    public SendOTPHandler(RepositoryOtp repositoryOtp, IEmailService emailService, ILogger<SendOTPHandler> logger)
    {
        _repositoryOtp = repositoryOtp;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<bool> Handle(SendOTPRequest request)
    {
        _logger.LogInformation("Attempting to send OTP for email: {Email}", request.email);

        var code = new Random().Next(111111, 999999).ToString();
        var expiry = DateTime.UtcNow.AddMinutes(5);

        var entityOtp = new EntityOtp
        {
            Email = request.email,
            Code = code,
            CreatedAt = DateTime.UtcNow,
            ExpiredAt = expiry,
            Status = OtpStatus.Pending
        };

        await _repositoryOtp.CreateAsync(entityOtp);

        var viewModel = new OtpEmailViewModel
        {
            AppName = "Diggie",
            Name = request.email,
            OtpCode = code,
            ExpiredMinutes = 5
        };

        var result = await _emailService.SendAsync(request.email, "OTP Verification", viewModel, "send-otp-template");

        if (result)
        {
            _logger.LogInformation("Successfully sent OTP to email: {Email}", request.email);
        }
        else
        {
            _logger.LogWarning("Failed to send OTP to email: {Email}", request.email);
        }

        return result;
    }

}
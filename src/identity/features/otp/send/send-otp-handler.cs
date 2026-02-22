using diggie_server.src.infrastructure.persistence.repositories;
using diggie_server.src.shared.email.models;

namespace diggie_server.src.identity.features.otp.send;

public class SendOTPHandler
{
    private readonly RepositoryOtp repositoryOtp;
    private readonly RepositoryUser repositoryUser;
    private readonly IEmailService emailService;
    private readonly ILogger<SendOTPHandler> logger;

    public SendOTPHandler(
        RepositoryOtp repositoryOtp,
        RepositoryUser repositoryUser,
        IEmailService emailService,
        ILogger<SendOTPHandler> logger
    )
    {
        this.repositoryOtp = repositoryOtp;
        this.repositoryUser = repositoryUser;
        this.emailService = emailService;
        this.logger = logger;
    }

    public async Task<bool> Handle(SendOTPRequest request)
    {
        logger.LogInformation("Attempting to send OTP for email: {Email}", request.email);

        var emailExists = await repositoryUser.IsEmailExists(request.email);
        if (!emailExists)
        {
            logger.LogWarning("Email does not exist: {Email}", request.email);
            return false;
        }

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

        await repositoryOtp.CreateAsync(entityOtp);

        var viewModel = new OtpEmailViewModel
        {
            AppName = "Diggie",
            Name = request.email.Split('@')[0],
            OtpCode = code,
            ExpiredMinutes = 5
        };

        var result = await emailService.SendAsync(
            request.email,
            "OTP Verification",
             viewModel,
            "send-otp-template"
            );

        if (result)
        {
            logger.LogInformation("Successfully sent OTP to email: {Email}", request.email);
        }
        else
        {
            logger.LogWarning("Failed to send OTP to email: {Email}", request.email);
        }

        return result;
    }

}
using diggie_server.src.infrastructure.persistence.repositories;

namespace diggie_server.src.identity.features.otp.verify;

public class VerifyOtpHandler
{
    private readonly RepositoryUser repositoryUser;
    private readonly RepositoryOtp repositoryOtp;
    private readonly ILogger<VerifyOtpHandler> logger;
    public VerifyOtpHandler(
        RepositoryUser repositoryUser,
        RepositoryOtp repositoryOtp,
        ILogger<VerifyOtpHandler> logger
        )
    {
        this.repositoryUser = repositoryUser;
        this.repositoryOtp = repositoryOtp;
        this.logger = logger;
    }

    public async Task<bool> Handle(VerifyOtpRequest request)
    {
        try
        {
            var user = await repositoryUser.GetByEmailAsync(request.Email);
            if (user == null)
            {
                logger.LogWarning("User not found: {Email}", request.Email);
                return false;
            }

            var otpRecord = await repositoryOtp.GetByEmailAsync(request.Email);
            if (otpRecord == null)
            {
                logger.LogWarning("OTP record not found for user: {Email}", request.Email);
                return false;
            }

            if (otpRecord.Code != request.Otp)
            {
                logger.LogWarning("Invalid OTP for user: {Email}", request.Email);
                return false;
            }

            if (otpRecord.Status != OtpStatus.Pending)
            {
                logger.LogWarning("OTP is not pending for user: {Email}", request.Email);
                return false;
            }

            if (otpRecord.ExpiredAt < DateTime.UtcNow)
            {
                logger.LogWarning("OTP expired for user: {Email}", request.Email);
                return false;
            }

            otpRecord.Status = OtpStatus.Verified;
            await repositoryOtp.UpdateAsync(otpRecord);

            await repositoryOtp.DeleteAsync(otpRecord);

            logger.LogInformation("OTP verified for user: {Email}", request.Email);
            return true;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error verifying OTP for user: {Email}", request.Email);
            return false;
        }
    }
}
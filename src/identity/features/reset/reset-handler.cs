using diggie_server.src.infrastructure.persistence.repositories;
using diggie_server.src.shared.validation;

namespace diggie_server.src.identity.features.reset;

public class ResetHandler
{
    private readonly RepositoryUser repositoryUser;
    private readonly ILogger<ResetHandler> logger;
    public ResetHandler(
        RepositoryUser repositoryUser,
        ILogger<ResetHandler> logger)
    {
        this.repositoryUser = repositoryUser;
        this.logger = logger;
    }

    public async Task<bool> Handle(ResetRequest request)
    {
        try
        {
            ValidationGuard.ValidateAuth(request.Email, request.NewPassword);

            var user = await repositoryUser.GetByEmailAsync(request.Email);
            if (user == null) throw new KeyNotFoundException("User tidak ditemukan");

            user.EnsureCanAccessSystem();

            if (request.NewPassword.Length < 8)
            {
                logger.LogWarning("Password too short for: {Email}", request.Email);
                return false;
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                logger.LogWarning("New password and confirm password do not match for: {Email}", request.Email);
                return false;
            }

            user.UpdatePassword(request.NewPassword);

            await repositoryUser.UpdateAsync(user);

            logger.LogInformation("Password successfully changed for: {Email}", request.Email);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to reset password for: {Email}", request.Email);
            return false;
        }
    }
}

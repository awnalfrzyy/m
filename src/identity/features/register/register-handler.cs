using diggie_server.src.infrastructure.persistence.repositories;
using diggie_server.src.infrastructure.persistence.entities;
using diggie_server.src.shared.validation;

namespace diggie_server.src.identity.features.register;

public class RegisterHandler
{
    private readonly RepositoryUser repositoryUser;
    private readonly ILogger<RegisterHandler> logger;
    public RegisterHandler(RepositoryUser repositoryUser, ILogger<RegisterHandler> logger)
    {
        this.repositoryUser = repositoryUser;
        this.logger = logger;
    }

    public async Task<RegisterResponse> Handle(RegisterRequest request)
    {
        ValidationGuard.ValidateAuth(request.Email, request.Password);

        await IdentityGuard.EnsureRegistrationIsUnique(request.Email, request.Name, repositoryUser);

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new EntityUser
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await repositoryUser.CreateUser(user);
        logger.LogInformation("User created with ID: {UserId}", createdUser.Id);
        return new RegisterResponse(createdUser.Id, createdUser.Name, createdUser.Email);
    }
}
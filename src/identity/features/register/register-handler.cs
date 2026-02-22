using diggie_server.src.infrastructure.persistence.repositories;
using diggie_server.src.infrastructure.persistence.entities;
using diggie_server.src.shared.validation;

namespace diggie_server.src.identity.features.register;

public class RegisterHandler
{
    private readonly RepositoryUser _repositoryUser;
    public RegisterHandler(RepositoryUser repositoryUser) => _repositoryUser = repositoryUser;
    public async Task<RegisterResponse> Handle(RegisterRequest request)
    {
        ValidationGuard.ValidateAuth(request.Email, request.Password);
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new EntityUser
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = hashedPassword
        };
        var createdUser = await _repositoryUser.CreateUser(user);
        return new RegisterResponse(createdUser.Id, createdUser.Name, createdUser.Email);
    }
}
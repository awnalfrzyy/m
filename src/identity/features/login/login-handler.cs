using diggie_server.src.infrastructure.persistence.repositories;
using diggie_server.src.infrastructure.auth.jwt;
using diggie_server.src.shared.validation;

namespace diggie_server.src.identity.features.login;

public class LoginHandler
{
    private readonly RepositoryUser _repositoryUser;
    private readonly ILogger<LoginHandler> _logger;
    private readonly JwtService _jwtService;

    public LoginHandler(RepositoryUser repositoryUser, ILogger<LoginHandler> logger, JwtService jwtService)
    {
        _repositoryUser = repositoryUser;
        _logger = logger;
        _jwtService = jwtService;
    }

    public async Task<(LoginResponse response, string token)> Handle(LoginRequest request)
    {

        ValidationGuard.ValidateAuth(request.email, request.password);

        _logger.LogDebug("Attempting login for email {Email}", request.email);

        var user = await _repositoryUser.GetByEmailAsync(request.email);
        if (user == null || !user.IsPasswordValid(request.password))
        {
            _logger.LogWarning("Login failed for email {Email}", request.email);
            throw new ArgumentException("Invalid email or password");
        }

        var token = _jwtService.Generate(user);

        _logger.LogInformation("Login successful for user {UserId}", user.Id);
        var response = new LoginResponse(user.Id, user.Email, user.Name);

        return (response, token);
    }
}
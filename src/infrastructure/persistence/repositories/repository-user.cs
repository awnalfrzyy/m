using diggie_server.src.infrastructure.persistence.entities;

namespace diggie_server.src.infrastructure.persistence.repositories;

public class RepositoryUser
{
    private readonly AppDatabaseContext _context;
    private readonly ILogger<RepositoryUser> _logger;

    public RepositoryUser(AppDatabaseContext context, ILogger<RepositoryUser> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EntityUser?> GetUserByIdAsync(Guid id)
    {
        _logger.LogDebug("GetUserByIdAsync called for {UserId}", id);
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            _logger.LogWarning("User {UserId} not found", id);
        else
            _logger.LogInformation("User {UserId} retrieved", id);

        return user;
    }

}
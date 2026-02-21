using diggie_server.src.infrastructure.persistence.entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<EntityUser> CreateUser(EntityUser user)
    {
        _logger.LogDebug("Creating user {UserName}", user.Name);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User {UserName} created", user.Name);
        return user;
    }

    public async Task<EntityUser?> GetByEmailAsync(string email)
    {
        _logger.LogDebug("GetUserByEmailAsync called for email {Email}", email);
        return await _context.Users
        .AsNoTracking()
        .Where(u => u.DeleteAt == null)
        .FirstOrDefaultAsync(u => u.Email == email);
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
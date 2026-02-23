using Microsoft.EntityFrameworkCore;
using diggie_server.src.infrastructure.persistence.entities;

namespace diggie_server.src.infrastructure.persistence.repositories;

public interface ICartRepository
{
    Task<EntityCart?> GetCartItem(Guid productId);
    Task AddCartItem(EntityCart cart);
    Task UpdateCartItem(EntityCart cart);
    Task<CreateCartResponse?> GetCartResponse(Guid productId);
}

public class RepositoryCart : ICartRepository
{
    private readonly AppDatabaseContext _context;

    public RepositoryCart(AppDatabaseContext context)
    {
        _context = context;
    }

    public async Task<EntityCart?> GetCartItem(Guid productId)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task AddCartItem(EntityCart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartItem(EntityCart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task<CreateCartResponse?> GetCartResponse(Guid productId)
    {
        return await _context.Carts
            .Where(c => c.ProductId == productId)
            .Select(c => new CreateCartResponse(
                c.Id,
                c.ProductId,
                c.Product.Name,
                c.Product.Image,
                c.Product.Price,
                c.Quantity,
                c.Product.Price * c.Quantity
            ))
            .FirstOrDefaultAsync();
    }
}
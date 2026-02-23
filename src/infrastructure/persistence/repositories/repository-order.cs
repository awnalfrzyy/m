using Microsoft.EntityFrameworkCore;
using diggie_server.src.infrastructure.persistence.entities;

namespace diggie_server.src.infrastructure.persistence.repositories;

public class RepositoryOrder
{
    private readonly AppDatabaseContext _context;
    private readonly ILogger<RepositoryOrder>? _logger;

    public RepositoryOrder(AppDatabaseContext context, ILogger<RepositoryOrder>? logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EntityOrder> CreateOrder(EntityOrder order)
    {
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Gagal simpan order ke database.");
            throw;
        }
    }

    public async Task<CreateOrderResponse?> GetOrderById(Guid id)
    {
        return await _context.Orders
            .Where(o => o.Id == id)
            .Select(o => new CreateOrderResponse(
                o.Id,
                o.TotalAmount,
                o.Status.ToString(),
                o.CreatedAt,
                o.OrderItems.Select(item => new OrderItemResponse(
                    item.ProductId,
                    item.Product.Name,
                    item.Quantity,
                    item.PriceAtPurchase
                )).ToList()
            ))
            .FirstOrDefaultAsync();
    }
}
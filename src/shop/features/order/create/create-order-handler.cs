namespace diggie_server.src.shop.features.order.create;

using diggie_server.src.infrastructure.persistence;
using diggie_server.src.infrastructure.persistence.entities;
using diggie_server.src.infrastructure.persistence.repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class CreateOrderHandler
{
    private readonly RepositoryOrder _orderRepo;
    private readonly AppDatabaseContext _context;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(
        RepositoryOrder orderRepo,
        AppDatabaseContext context,
        ILogger<CreateOrderHandler> logger)
    {
        _orderRepo = orderRepo;
        _context = context;
        _logger = logger;
    }

    public async Task<CreateOrderResponse> handle(CreateOrderRequest request)
    {
        _logger.LogInformation("Memulai proses Checkout untuk ProductId: {ProductId}", request.ProductId);

        var cartItems = await _context.Carts
            .Include(c => c.Product)
            .ToListAsync();

        if (!cartItems.Any())
        {
            _logger.LogWarning("Checkout gagal: Keranjang kosong.");
            throw new Exception("Keranjang lu kosong, Win!");
        }

        _logger.LogInformation("Ditemukan {Count} item di keranjang. Memulai transaksi database...", cartItems.Count);

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var order = new EntityOrder
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = cartItems.Sum(x => x.Product.Price * x.Quantity),
                OrderItems = cartItems.Select(cart => new EntityOrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                    PriceAtPurchase = cart.Product.Price
                }).ToList()
            };

            _logger.LogInformation("Menyimpan Order {OrderId} dengan {ItemCount} item.", order.Id, order.OrderItems.Count);
            await _orderRepo.CreateOrder(order);

            _logger.LogInformation("Membersihkan data keranjang...");
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            _logger.LogInformation("Checkout berhasil untuk OrderId: {OrderId}", order.Id);

            var response = await _orderRepo.GetOrderById(order.Id);
            return response ?? throw new Exception("Gagal generate response order.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Terjadi error fatal saat Checkout. Melakukan Rollback transaksi.");
            await transaction.RollbackAsync();
            throw;
        }
    }
}
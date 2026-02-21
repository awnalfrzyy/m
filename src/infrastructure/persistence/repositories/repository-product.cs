using diggie_server.src.infrastructure.persistence.entities;
using Microsoft.EntityFrameworkCore;

namespace diggie_server.src.infrastructure.persistence.repositories;

public class ProductRepository
{
    private readonly AppDatabaseContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(AppDatabaseContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(EntityProduct product)
    {
        _logger.LogDebug("AddAsync called");

        if (product == null)
        {
            _logger.LogError("AddAsync called with null product");
            throw new ArgumentNullException(nameof(product));
        }

        if (product.Id == Guid.Empty)
            product.Id = Guid.NewGuid();

        product.Status = ProductStatus.Active;
        product.CreatedAt = DateTime.UtcNow;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product created with Id {ProductId}", product.Id);
    }

    public async Task<IEnumerable<EntityProduct>> GetAllAsync()
    {
        _logger.LogDebug("GetAllAsync called");

        var products = await _context.Products
            .AsNoTracking()
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} products", products.Count);
        return products;
    }

    public async Task<EntityProduct> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("GetByIdAsync called for {ProductId}", id);

        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", id);
            throw new KeyNotFoundException($"Product dengan ID {id} tidak ditemukan.");
        }

        _logger.LogInformation("Product {ProductId} retrieved", id);
        return product;
    }

    public async Task<EntityProduct> UpdateByIdAsync(Guid id, EntityProduct product)
    {
        _logger.LogDebug("UpdateByIdAsync called for {ProductId}", id);

        var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (existingProduct == null)
        {
            _logger.LogWarning("Update failed: product {ProductId} not found", id);
            throw new KeyNotFoundException($"Product dengan ID {id} tidak ditemukan.");
        }

        product.Id = id;

        _context.Entry(existingProduct).CurrentValues.SetValues(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product {ProductId} updated", id);
        return existingProduct;
    }

    public async Task<EntityProduct> DeleteByIdAsync(Guid id)
    {
        _logger.LogDebug("DeleteByIdAsync called for {ProductId}", id);

        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Delete failed: product {ProductId} not found", id);
            throw new KeyNotFoundException($"Product dengan ID {id} tidak ditemukan.");
        }

        product.Status = ProductStatus.Deleted;
        product.DeleteAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Product {ProductId} marked as deleted", id);

        return product;
    }
}
using diggie_server.src.features.product.entity;
using Microsoft.EntityFrameworkCore;

namespace diggie_server.src.infrastructure.persistence;

public class ProductRepository
{
    private readonly AppDatabaseContext _context;
    public ProductRepository(AppDatabaseContext context) => _context = context;

    public async Task AddAsync(EntityProduct product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<EntityProduct>> GetAllAsync()
    {
        return await _context.Products
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<EntityProduct> GetByIdAsync(Guid id)
    {
        var product = await _context.Products
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            throw new KeyNotFoundException($"Product dengan ID {id} tidak ditemukan.");

        return product;
    }

    public async Task<EntityProduct> UpdateByIdAsync(Guid id, EntityProduct product)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (existingProduct == null)
            throw new KeyNotFoundException($"Product dengan ID {id} tidak ditemukan.");

        product.Id = id;

        _context.Entry(existingProduct).CurrentValues.SetValues(product);
        await _context.SaveChangesAsync();

        return existingProduct;
    }

    public async Task<EntityProduct> DeleteByIdAsync(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            throw new KeyNotFoundException($"Product dengan ID {id} tidak ditemukan.");

        product.Status = ProductStatus.Deleted;
        product.DeleteAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return product;
    }
}
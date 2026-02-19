using diggie_server.src.features.product;
using diggie_server.src.infrastructure.persistence;

public class ProductRepository
{
    private readonly AppDatabaseContext _context;
    public ProductRepository(AppDatabaseContext context) => _context = context;

    public async Task AddAsync(EntityProduct product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
}
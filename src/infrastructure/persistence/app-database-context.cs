using Microsoft.EntityFrameworkCore;
using diggie_server.src.infrastructure.persistence.entities;
namespace diggie_server.src.infrastructure.persistence
{
    public class AppDatabaseContext : DbContext
    {
        private readonly ILogger<AppDatabaseContext>? _logger;

        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options, ILogger<AppDatabaseContext>? logger = null)
            : base(options)
        {
            _logger = logger;
        }

        public DbSet<EntityProduct> Products { get; set; }
        public DbSet<EntityPlan> Plans { get; set; }
        public DbSet<EntityUser> Users { get; set; }
        public DbSet<EntityOtp> Otps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EntityProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasQueryFilter(e => e.DeleteAt == null);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Image).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(ProductStatus.Active);
                entity.Property(e => e.DeleteAt).IsRequired(false);
            });
            modelBuilder.Entity<EntityPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Durations).IsRequired();
                entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<EntityUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasQueryFilter(e => e.DeleteAt == null);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Gender)
                    .HasConversion<string>();
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(StatusUser.Active);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.DeleteAt).IsRequired(false);
            });
            modelBuilder.Entity<EntityOtp>(entity =>
            {
                entity.HasKey(e => e.Email);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Code).IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.ExpiredAt).IsRequired();
                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(OtpStatus.Pending);
            });
        }

        public override int SaveChanges()
        {
            _logger?.LogDebug("SaveChanges called");
            try
            {
                var result = base.SaveChanges();
                _logger?.LogInformation("SaveChanges completed, {Count} entries written.", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "SaveChanges failed");
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("SaveChangesAsync called");
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                _logger?.LogInformation("SaveChangesAsync completed, {Count} entries written.", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "SaveChangesAsync failed");
                throw;
            }
        }
    }
}

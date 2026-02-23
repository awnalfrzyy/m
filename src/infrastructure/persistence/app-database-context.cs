using Microsoft.EntityFrameworkCore;
using diggie_server.src.infrastructure.persistence.entities;
namespace diggie_server.src.infrastructure.persistence
{
    public class AppDatabaseContext : DbContext
    {
        private readonly ILogger<AppDatabaseContext>? logger;

        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options, ILogger<AppDatabaseContext>? logger = null)
            : base(options)
        {
            this.logger = logger;
        }

        public DbSet<EntityProduct> Products { get; set; }
        public DbSet<EntityPlan> Plans { get; set; }
        public DbSet<EntityUser> Users { get; set; }
        public DbSet<EntityOtp> Otps { get; set; }
        public DbSet<EntityOrder> Orders { get; set; }
        public DbSet<EntityOrderItem> OrderItems { get; set; }
        public DbSet<EntityHistory> History { get; set; }
        public DbSet<EntityCart> Carts { get; set; }

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
            modelBuilder.Entity<EntityOrder>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.Status)
                     .HasConversion<string>()
                     .HasDefaultValue(OrderStatus.Pending);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.DeleteAt).IsRequired(false);
            });
            modelBuilder.Entity<EntityOrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.OrderId).IsRequired();
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.PriceAtPurchase).IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.DeleteAt).IsRequired(false);
            });


            modelBuilder.Entity<EntityHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Product).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.MetodePayments).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(StatusPayments.Pending);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.DeleteAt).IsRequired(false);
            });
            modelBuilder.Entity<EntityCart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.AddedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.DeleteAt).IsRequired(false);
            });

        }

        public override int SaveChanges()
        {
            logger?.LogDebug("SaveChanges called");
            try
            {
                var result = base.SaveChanges();
                logger?.LogInformation("SaveChanges completed, {Count} entries written.", result);
                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "SaveChanges failed");
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            logger?.LogDebug("SaveChangesAsync called");
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                logger?.LogInformation("SaveChangesAsync completed, {Count} entries written.", result);
                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "SaveChangesAsync failed");
                throw;
            }
        }
    }
}

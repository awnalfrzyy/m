using Microsoft.EntityFrameworkCore;
using diggie_server.src.features.product;
using diggie_server.src.features.plan;

namespace diggie_server.src.infrastructure.persistence
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<EntityProduct> Products { get; set; }
        public DbSet<EntityPlan> Plans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EntityProduct>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Image).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Brand).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.CreatedAt).ValueGeneratedOnAdd();
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
        }
    }
}

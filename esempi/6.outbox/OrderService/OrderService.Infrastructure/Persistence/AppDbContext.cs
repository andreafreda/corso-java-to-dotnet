// Infrastructure/Persistence/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using OrderService.Core.Entities;

namespace OrderService.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order>         Orders { get; set; }
    public DbSet<OutboxMessage> Outbox { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Customer).IsRequired().HasMaxLength(100);
            entity.Property(o => o.Total).HasColumnType("decimal(18,2)");
            entity.Property(o => o.Status).HasConversion<string>();
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("Outbox");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Payload).HasColumnType("nvarchar(max)");
        });
    }
}

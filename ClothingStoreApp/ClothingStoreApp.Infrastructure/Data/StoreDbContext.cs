using ClothingStoreApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreApp.Infrastructure.Data;
public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrdersProducts> OrdersProducts { get; set; }
    public DbSet<MethodType> MethodTypes { get; set; }
    public DbSet<HttpLog> HttpLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrdersProducts>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrdersProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrdersProducts>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrdersProducts)
            .HasForeignKey(op => op.ProductId);

        modelBuilder.Entity<MethodType>()
            .HasKey(m => m.MethodId);

        modelBuilder.Entity<HttpLog>()
            .HasKey(log => log.Id);

        modelBuilder.Entity<MethodType>()
            .Property(m => m.MethodId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<MethodType>().HasData(
            new MethodType { MethodId = 1, MethodTypeName = "GET" },
            new MethodType { MethodId = 2, MethodTypeName = "POST" },
            new MethodType { MethodId = 3, MethodTypeName = "DELETE" },
            new MethodType { MethodId = 4, MethodTypeName = "PUT" }
        );
    }
}
using ClothingStoreApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreApp.Infrastructure.Data;

public class StoreDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrdersProducts> OrdersProducts { get; set; }
    public DbSet<MethodType> MethodTypes { get; set; }
    public DbSet<HttpLog> HttpLogs { get; set; }
    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasPrecision(18, 2);

        builder.Entity<OrdersProducts>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrdersProducts)
            .HasForeignKey(op => op.OrderId);

        builder.Entity<OrdersProducts>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrdersProducts)
            .HasForeignKey(op => op.ProductId);

        builder.Entity<MethodType>()
            .HasKey(m => m.MethodId);

        builder.Entity<HttpLog>()
            .HasKey(log => log.Id);

        builder.Entity<MethodType>()
            .Property(m => m.MethodId)
            .ValueGeneratedOnAdd();

        builder.Entity<MethodType>().HasData(
            new MethodType { MethodId = 1, MethodTypeName = "GET" },
            new MethodType { MethodId = 2, MethodTypeName = "POST" },
            new MethodType { MethodId = 3, MethodTypeName = "DELETE" },
            new MethodType { MethodId = 4, MethodTypeName = "PUT" }
        );

        builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Classic White T‑Shirt", Description = "Soft cotton tee", Price = 19.99M, ImageUrl = "https://plus.unsplash.com/premium_photo-1718913936342-eaafff98834b?q=80&w=1744&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
            new Product { Id = 2, Name = "Denim Jacket", Description = "Stylish blue denim jacket", Price = 59.99M, ImageUrl = "https://images.unsplash.com/photo-1537465978529-d23b17165b3b?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
            new Product { Id = 3, Name = "Black Hoodie", Description = "Cozy fleece hoodie", Price = 39.50M, ImageUrl = "https://plus.unsplash.com/premium_photo-1673356302169-574db56b52cd?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
            new Product { Id = 4, Name = "Slim Fit Jeans", Description = "Modern slim fit stretch jeans", Price = 44.95M, ImageUrl = "https://plus.unsplash.com/premium_photo-1691367279313-71af7ba83f2d?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
            new Product { Id = 5, Name = "Running Sneakers", Description = "Lightweight everyday sneakers", Price = 69.99M, ImageUrl = "https://images.unsplash.com/photo-1600185365483-26d7a4cc7519?q=80&w=1450&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
            new Product { Id = 6, Name = "Baseball Cap", Description = "Adjustable cotton cap", Price = 14.99M, ImageUrl = "https://images.unsplash.com/photo-1530398257477-3e1b0b0ed605?q=80&w=1942&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
            new Product { Id = 7, Name = "Summer Dress", Description = "Light floral summer dress", Price = 34.99M, ImageUrl = "https://images.unsplash.com/photo-1520026582657-4daf5bb60adb?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
            new Product { Id = 8, Name = "Formal Shirt", Description = "Classic button-down shirt", Price = 29.99M, ImageUrl = "https://images.unsplash.com/photo-1602810316693-3667c854239a?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" },
            new Product { Id = 9, Name = "Leather Belt", Description = "Durable genuine leather belt", Price = 24.99M, ImageUrl = "https://images.unsplash.com/photo-1664286074176-5206ee5dc878?q=80&w=1740&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" }
        );

    }
}
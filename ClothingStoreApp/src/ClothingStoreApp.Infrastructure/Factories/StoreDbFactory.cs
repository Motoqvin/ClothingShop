using ClothingStoreApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClothingStoreApp.Infrastructure.Factories;

public class StoreDbContextFactory : IDesignTimeDbContextFactory<StoreDbContext>
{
    StoreDbContext IDesignTimeDbContextFactory<StoreDbContext>.CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=StoreDB;Username=postgres;Password=Admin123!");

        return new StoreDbContext(optionsBuilder.Options);
    }
}
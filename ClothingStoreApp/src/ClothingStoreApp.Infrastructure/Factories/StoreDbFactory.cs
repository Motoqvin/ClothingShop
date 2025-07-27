using ClothingStoreApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClothingStoreApp.Infrastructure.Factories;

public class StoreDbContextFactory : IDesignTimeDbContextFactory<StoreDbContext>
{
    StoreDbContext IDesignTimeDbContextFactory<StoreDbContext>.CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StoreDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=StoreDB;Integrated Security=True;TrustServerCertificate=True;");

        return new StoreDbContext(optionsBuilder.Options);
    }
}
using ClothingStoreApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreApp.Infrastructure.Factories;
public class UsersDbFactory
{
    public UsersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

            optionsBuilder.UseSqlServer("Server=localhost;Database=IdentityDB;Integrated Security=True;TrustServerCertificate=True;");

            return new UsersDbContext(optionsBuilder.Options);
        }
}
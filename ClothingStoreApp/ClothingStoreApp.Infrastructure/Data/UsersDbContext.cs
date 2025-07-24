using ClothingStoreApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreApp.Infrastructure.Data;
public class UsersDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
        this.Database.EnsureCreated();
    }
}
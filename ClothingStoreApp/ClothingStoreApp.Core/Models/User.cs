using Microsoft.AspNetCore.Identity;

namespace ClothingStoreApp.Core.Models;

public class User : IdentityUser
{
    public override string? UserName { get => base.UserName; set => base.UserName = value; }
    public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
    public override string? Email { get => base.Email; set => base.Email = value; }
    public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
}
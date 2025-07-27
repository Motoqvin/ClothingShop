namespace ClothingStoreApp.Core.Models;

public class LoginViewModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool RememberMe { get; set; } = false;
    public string? ReturnUrl { get; set; }
}
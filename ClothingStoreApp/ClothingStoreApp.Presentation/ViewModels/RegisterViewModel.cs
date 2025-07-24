namespace ClothingStoreApp.Presentation.ViewModels;

public class RegisterViewModel
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public IFormFile? Avatar { get; set; }
    public required string Address { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PasswordHash { get; set; }
}
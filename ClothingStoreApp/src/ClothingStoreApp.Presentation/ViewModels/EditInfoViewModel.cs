namespace ClothingStoreApp.Presentation.ViewModels;

public class EditInfoViewModel
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public IFormFile? Avatar { get; set; }
    public string? CurrentAvatar { get; set; }
}

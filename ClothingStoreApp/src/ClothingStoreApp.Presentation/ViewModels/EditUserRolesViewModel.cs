namespace ClothingStoreApp.Presentation.ViewModels;
public class EditUserRolesViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> AvailableRoles { get; set; } = new();
    public List<string>? SelectedRoles { get; set; } = new();
}
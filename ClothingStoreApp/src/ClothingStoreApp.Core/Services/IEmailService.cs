using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Services;
public interface IEmailService
{
    Task SendOrderConfirmationEmailAsync(string toEmail, Order order);
}
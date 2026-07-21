using System.Net;
using System.Net.Mail;
using System.Text;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Core.Services;
using ClothingStoreApp.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClothingStoreApp.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings settings;
    private readonly ILogger<SmtpEmailService> logger;

    public SmtpEmailService(IOptions<SmtpSettings> options, ILogger<SmtpEmailService> logger)
    {
        this.settings = options.Value;
        this.logger = logger;
    }

    public async Task SendOrderConfirmationEmailAsync(string toEmail, Order order)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
        {
            logger.LogWarning("Skipped order confirmation email for order {OrderId}: recipient address is empty.", order.Id);
            return;
        }

        Console.WriteLine($"Host: {settings.Host}");
        Console.WriteLine($"Port: {settings.Port}");
        Console.WriteLine($"User: {settings.Username}");
        Console.WriteLine($"SSL: {settings.EnableSsl}");
        var body = BuildBody(order);

        using var message = new MailMessage
        {
            From = new MailAddress(settings.FromEmail, settings.FromName),
            Subject = $"Order Confirmation - #{order.Id}",
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(toEmail);

        using var client = new SmtpClient(settings.Host, settings.Port)
        {
            Credentials = new NetworkCredential(settings.Username, settings.Password),
            EnableSsl = settings.EnableSsl
        };

        try
        {
            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send order confirmation email for order {OrderId}.", order.Id);
        }
    }

    private static string BuildBody(Order order)
    {
        var sb = new StringBuilder();
        sb.Append("<h2>Thanks for your order!</h2>");
        sb.Append($"<p>Order #{order.Id} placed on {order.Date:yyyy-MM-dd HH:mm} has been confirmed.</p>");

        if (order.OrdersProducts != null && order.OrdersProducts.Count > 0)
        {
            sb.Append("<table style=\"border-collapse:collapse\" cellpadding=\"6\">");
            sb.Append("<tr><th align=\"left\">Product</th><th align=\"left\">Qty</th></tr>");
            foreach (var line in order.OrdersProducts)
            {
                var name = line.Product?.Name ?? $"Product #{line.ProductId}";
                sb.Append($"<tr><td>{name}</td><td>{line.Quantity}</td></tr>");
            }
            sb.Append("</table>");
        }

        sb.Append($"<p><strong>Total: {order.TotalPrice:C}</strong></p>");
        return sb.ToString();
    }
}

namespace ClothingStoreApp.Core.Responses;
public class BadRequestResponse
{
    public string? Parameter { get; set; }
    public string Message { get; set; }

    public BadRequestResponse(string param, string message)
    {
        this.Message = message;
        this.Parameter = param;
    }

    public BadRequestResponse(string message)
    {
        this.Message = message;
    }
}
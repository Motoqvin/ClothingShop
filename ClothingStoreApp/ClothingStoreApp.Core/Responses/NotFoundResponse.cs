namespace ClothingStoreApp.Core.Responses;
public class NotFoundResponse
{
    public string Message { get; set; }
    public NotFoundResponse(string message)
    {
        this.Message = message;
    }
}
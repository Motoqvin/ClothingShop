using ClothingStoreApp.Models;

namespace ClothingStoreApp.Services.Base;
public interface IHttpLogger
{
    public Task LogAsync(HttpLog log);
}
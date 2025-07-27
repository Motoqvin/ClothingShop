using ClothingStoreApp.Core.Models;

namespace ClothingStoreApp.Core.Services;
public interface IHttpLogger
{
    void Log(HttpLog log);
}
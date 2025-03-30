using ClothingStoreApp.Models;
using ClothingStoreApp.Repositories.Base;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ClothingStoreApp.Repositories;

public class HttpLogSqlRepository : IHttpLogRepository
{
    private readonly string ConnStr;
    public HttpLogSqlRepository(string connStr)
    {
        ConnStr = connStr;
    }
    public async Task InsertAsync(HttpLog log)
    {
        using var conn = new SqlConnection(ConnStr);
        await conn.OpenAsync();

        await conn.ExecuteAsync(
                sql:    $@"insert into HttpLog (RequestId, Url, RequestBody, RequestHeaders, MethodType, ResponseBody, ResponseHeaders, StatusCode, CreationDateTime, EndDateTime, ClientIp)
                        values (@RequestId, @Url, @RequestBody, @RequestHeaders, @MethodType, @ResponseBody, @ResponseHeaders, @StatusCode, @CreationDateTime, @EndDateTime, @ClientIp);",
                param: log);
    }
}
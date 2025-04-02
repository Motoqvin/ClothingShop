using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Core.Services;
using ClothingStoreApp.Presentation.Middlewares;
using ClothingStoreApp.Infrastructure.Repositories;
using ClothingStoreApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProductsRepository>((serviceProvider) => {
    var connStr = builder.Configuration.GetConnectionString("SqlDB") ?? "";
    return new ProductsDapperRepository(connStr);
});

builder.Services.AddTransient<IOrdersRepository>((serviceProvider) => {
    var connStr = builder.Configuration.GetConnectionString("SqlDB") ?? "";
    return new OrdersDapperRepository(connStr);
});

builder.Services.AddTransient<IHttpLogRepository>((serviceProvider) => {
    var connStr = builder.Configuration.GetConnectionString("SqlDB") ?? "";
    return new HttpLogSqlRepository(connStr);
});

builder.Services.AddScoped<IHttpLogger, HttpLogger>();


builder.Services.AddTransient<IProductService>((serviceProvider) => {
    var repo = serviceProvider.GetRequiredService<IProductsRepository>();
    return new ProductService(repo);
});
builder.Services.AddTransient<IOrdersService>((serviceProvider) => {
    var repo = serviceProvider.GetRequiredService<IOrdersRepository>();
    return new OrdersService(repo);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

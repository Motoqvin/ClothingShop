using InitProject.Repositories;
using InitProject.Repositories.Base;
using InitProject.Services;
using InitProject.Services.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Dapper
// builder.Services.AddTransient<IProductsRepository>((serviceProvider) => {
//     var connStr = "Server=localhost;Database=ProductsDB;Integrated Security=True;TrustServerCertificate=True;";
//     return new ProductsDapperRepository(connStr);
// });

// JSON
builder.Services.AddTransient<IProductsRepository>((serviceProvider) => {
    var path = builder.Configuration.GetSection("JSON_Path")?.Value ?? "smth";
    return new ProductsJsonRepository(path);
});
builder.Services.AddTransient<IProductService>((serviceProvider) => {
    var repo = serviceProvider.GetRequiredService<IProductsRepository>();
    return new ProductService(repo);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();

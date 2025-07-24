using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Core.Services;
using ClothingStoreApp.Presentation.Middlewares;
using ClothingStoreApp.Infrastructure.Repositories;
using ClothingStoreApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Infrastructure.Data;
using FluentValidation.AspNetCore;
using FluentValidation;
using ClothingStoreApp.Presentation.Validators;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IProductsRepository, ProductsEFRepository>();

builder.Services.AddTransient<IOrdersRepository, OrdersEFRepository>();

builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDB")));

builder.Services.AddDbContext<UsersDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("IdentityDb");
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<UsersDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IHttpLogRepository, HttpLogEFRepository>();

builder.Services.AddScoped<IHttpLogger, HttpLogger>();

builder.Services.AddDataProtection();

builder.Services.AddTransient<IProductService>((serviceProvider) => {
    var repo = serviceProvider.GetRequiredService<IProductsRepository>();
    return new ProductService(repo);
});
builder.Services.AddTransient<IOrdersService>((serviceProvider) =>
{
    var repo = serviceProvider.GetRequiredService<IOrdersRepository>();
    return new OrdersService(repo);
});

builder.Services.AddAuthentication(defaultScheme: CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
        authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme,
        configureOptions: options =>
        {
            options.LoginPath = "/Identity/Login";
            options.LogoutPath = "/Identity/Logout";
        });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(name: "MyPolicy", configurePolicy: policyBuilder => {
            policyBuilder
                .RequireRole("Admin");
        }
);

builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddValidatorsFromAssemblies([
    Assembly.GetExecutingAssembly(),
]);

var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

await roleManager.CreateAsync(new IdentityRole {Name = "Admin"});
await roleManager.CreateAsync(new IdentityRole {Name = "User"});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");

app.Run();

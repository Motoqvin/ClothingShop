using ClothingStoreApp.Core.Repositories;
using ClothingStoreApp.Core.Services;
using ClothingStoreApp.Presentation.Middlewares;
using ClothingStoreApp.Infrastructure.Repositories;
using ClothingStoreApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Infrastructure.Data;
using ClothingStoreApp.Core.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IProductsRepository, ProductsEFRepository>();

builder.Services.AddTransient<IOrdersRepository, OrdersEFRepository>();
Console.WriteLine(builder.Configuration.GetConnectionString("IdentityDb"));
builder.Services.AddDbContext<StoreDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("IdentityDb");
    options.UseNpgsql(connectionString);
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<StoreDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<IEmailService, SmtpEmailService>();

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
    var productRepo = serviceProvider.GetRequiredService<IProductsRepository>();
    return new OrdersService(repo, productRepo);
});

builder.Services.AddAuthentication(defaultScheme: CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
        authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme,
        configureOptions: options =>
        {
            options.LoginPath = "/Identity/Login";
            options.LogoutPath = "/Identity/Logout";
        });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/Identity/Login";
    options.LogoutPath = "/Identity/Logout";
    options.AccessDeniedPath = "/Identity/AccessDenied";
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(name: "MyPolicy", configurePolicy: policyBuilder =>
    {
        policyBuilder
            .RequireRole("Admin");
    }
);

builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblies([
    Assembly.GetExecutingAssembly(),
]);

var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

static async Task SeedAdmin(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole {Name = "User"});
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    var adminEmail = "admin@store.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = "admin",
            Email = adminEmail,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    await dbContext.Database.MigrateAsync();
    await SeedAdmin(scope.ServiceProvider);
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

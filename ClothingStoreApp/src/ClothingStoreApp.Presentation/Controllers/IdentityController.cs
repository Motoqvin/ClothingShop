using ClothingStoreApp.Infrastructure.Data;
using ClothingStoreApp.Core.Dtos;
using ClothingStoreApp.Core.Enums;
using ClothingStoreApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ClothingStoreApp.Presentation.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace ClothingStoreApp.Presentation.Controllers;

[Route("[controller]/[action]")]
public class IdentityController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly StoreDbContext dbContext;
    private readonly IWebHostEnvironment _environment;

    public IdentityController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager,
        StoreDbContext dbContext,
        IWebHostEnvironment environment
        )
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.dbContext = dbContext;
        this._environment = environment;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Registration()
    {
        var errorMessage = TempData["Error"];
        if (errorMessage != null)
        {
            ModelState.AddModelError(string.Empty, errorMessage.ToString()!);
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registration([FromForm] RegisterViewModel newUser)
    {
        if (!ModelState.IsValid)
        {
            System.Console.WriteLine("Model state is not valid");
            return View(newUser);
        }
        try
        {
            string? avatarPath = null;

            if (newUser.Avatar != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(newUser.Avatar.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await newUser.Avatar.CopyToAsync(stream);

                avatarPath = $"uploads/avatars/{fileName}";
            }

            var user = new User
            {
                UserName = newUser.Username,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                Address = newUser.Address,
                Avatar = avatarPath
            };

            var result = await this.userManager.CreateAsync(user, newUser.PasswordHash!);

            System.Console.WriteLine(result);
            if (result.Succeeded)
            {
                await this.userManager.AddToRoleAsync(user, nameof(Roles.User));
                return RedirectToAction(nameof(Login));
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                newUser.PasswordHash = string.Empty;
                return View(newUser);
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            TempData["Error"] = "Something went wrong...";
            return RedirectToAction("Registration");
        }
    }

    [HttpGet]
    public async Task<IActionResult> MyInfo()
    {
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null)
            return RedirectToAction("Login");

        var userModel = new User
        {
            UserName = currentUser.UserName,
            Email = currentUser.Email,
            PhoneNumber = currentUser.PhoneNumber,
            Address = currentUser.Address,
            Avatar = currentUser.Avatar
        };

        return View(userModel);
    }

    [HttpGet]
    public async Task<IActionResult> EditInfo()
    {
        var identityUser = await userManager.GetUserAsync(User);
        if (identityUser == null)
            return RedirectToAction("Login");

        var model = new EditInfoViewModel
        {
            UserName = identityUser.UserName!,
            Email = identityUser.Email!,
            PhoneNumber = identityUser.PhoneNumber,
            Address = identityUser.Address,
            CurrentAvatar = identityUser.Avatar
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditInfo(EditInfoViewModel updatedUser)
    {
        var identityUser = await userManager.GetUserAsync(User);
        if (identityUser == null)
            return RedirectToAction("Login");

        if (!ModelState.IsValid)
        {
            updatedUser.CurrentAvatar = identityUser.Avatar;
            return View(updatedUser);
        }

        identityUser.UserName = updatedUser.UserName;
        identityUser.Email = updatedUser.Email;
        identityUser.PhoneNumber = updatedUser.PhoneNumber;
        identityUser.Address = updatedUser.Address;

        if (updatedUser.Avatar != null && updatedUser.Avatar.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(updatedUser.Avatar.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await updatedUser.Avatar.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(identityUser.Avatar))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", identityUser.Avatar);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            identityUser.Avatar = $"uploads/avatars/{fileName}";
        }

        var result = await userManager.UpdateAsync(identityUser);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            updatedUser.CurrentAvatar = identityUser.Avatar;
            return View(updatedUser);
        }

        await signInManager.RefreshSignInAsync(identityUser);

        return RedirectToAction(nameof(MyInfo));
    }



    [HttpGet]
    public ActionResult Login(string? returnUrl)
    {
        var errorMessage = base.TempData["Error"];

        if (string.IsNullOrWhiteSpace(returnUrl) == false)
        {
            base.ViewData["returnUrl"] = returnUrl;
        }

        if (errorMessage != null)
        {
            base.ModelState.AddModelError(string.Empty, errorMessage.ToString()!);
        }

        return base.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login([FromForm] LoginDto dto)
    {
        var foundUser = await this.userManager.FindByEmailAsync(dto.Email);

        if (foundUser == null)
        {
            TempData["Error"] = "Invalid email or password.";
            return base.RedirectToAction(actionName: nameof(Login), routeValues: new { returnUrl = dto.ReturnUrl });
        }

        var signInResult = await signInManager.PasswordSignInAsync(foundUser, dto.Password, dto.RememberMe, true);

        if (signInResult.Succeeded == false)
        {
            TempData["Error"] = signInResult.IsLockedOut
                ? "Your account has been locked due to multiple failed login attempts. Please try again later."
                : "Invalid email or password.";
            return base.RedirectToAction(actionName: nameof(Login), routeValues: new { returnUrl = dto.ReturnUrl });
        }


        if (string.IsNullOrWhiteSpace(dto.ReturnUrl) == false)
        {
            return base.Redirect(dto.ReturnUrl);
        }

        return base.RedirectToAction(actionName: "Index", controllerName: "Home");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAvatar()
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null)
            return NotFound();

        if (!string.IsNullOrEmpty(user.Avatar))
        {
            var filePath = Path.Combine(
                _environment.WebRootPath,
                user.Avatar);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            user.Avatar = null;

            await userManager.UpdateAsync(user);
        }

        TempData["Success"] = "Avatar deleted successfully.";

        return RedirectToAction(nameof(EditInfo));
    }

    [HttpGet]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return base.RedirectToAction(actionName: "Welcome", controllerName: "Home");
    }
}
using Microsoft.AspNetCore.Mvc;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Presentation.Extensions;
using Microsoft.AspNetCore.Identity;
using ClothingStoreApp.Core.Services;

public class CartController : Controller
{
    private const string SessionKey = "Cart";
    private readonly UserManager<User> userManager;
    private readonly IOrdersService ordersService;

    public CartController(UserManager<User> userManager, IOrdersService ordersService)
    {
        this.ordersService = ordersService;
        this.userManager = userManager;
    }

    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    [HttpPost]
    public IActionResult Add(int id, string name, decimal price)
    {
        var cart = GetCart();

        var item = cart.FirstOrDefault(p => p.ProductId == id);
        if (item != null)
        {
            item.Quantity++;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = id,
                ProductName = name,
                Price = price,
                Quantity = 1
            });
        }

        SaveCart(cart);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var cart = GetCart();

        if (cart.Count == 0)
        {
            TempData["Error"] = "Cart is empty.";
            return RedirectToAction("Index");
        }

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToAction("Login", "Identity");
        }

        var order = new Order
        {
            UserId = user.Id,
            TotalPrice = cart.Sum(i => i.Price * i.Quantity),
            OrdersProducts = [.. cart.Select(item => new OrdersProducts
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            })]
        };

        ordersService.SendOrder(order);

        HttpContext.Session.Remove(SessionKey);
        return View("OrderSuccess");
    }


    [HttpPost]
    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(p => p.ProductId == id);
        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }
        return RedirectToAction("Index");
    }

    private List<CartItem> GetCart()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>(SessionKey);
        return cart ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetObject(SessionKey, cart);
    }
}

using Microsoft.AspNetCore.Mvc;
using ClothingStoreApp.Core.Models;
using ClothingStoreApp.Presentation.Extensions;

public class CartController : Controller
{
    private const string SessionKey = "Cart";

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
    public IActionResult Checkout()
    {
        var cart = GetCart();
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

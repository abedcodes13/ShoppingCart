using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Services;
using System.Security.Claims;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CartController(ApplicationDbContext context, CartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            ViewBag.totalcost = cart.CartItems.Sum(ci => ci.Item.Price * ci.Quantity);
            return View(cart.CartItems);
        }

        // GET: Cart/Add/5
        public async Task<IActionResult> Add(int id)
        {
            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            if (item.Quantity > 0)
            {
                item.Quantity--;
                await _context.SaveChangesAsync();
                _cartService.AddItem(item, 1);
            }

            return RedirectToAction("Index");
        }

        // GET: Cart/Minus/5
        public async Task<IActionResult> Minus(int id)
        {
            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            int whatsLeft = _cartService.RemoveItem(id);

            item.Quantity++;
            await _context.SaveChangesAsync();

            if (whatsLeft > 0) return RedirectToAction("Index");
            else return RedirectToAction("Index", "Items");
        }

        // GET: Cart/Minus/5
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            int total = _cartService.TotalItem(id);

            _cartService.RemoveItem(id, total);

            item.Quantity += total;
            await _context.SaveChangesAsync();

            if (_cartService.CartCount() > 0) return RedirectToAction("Index");
            else return RedirectToAction("Index", "Items");
        }

        // GET: Cart/Checkout
        // GET: Cart/Checkout
        public async Task<IActionResult> Checkout()
        {
            var cart = _cartService.GetCart();

            // Iterate over cart items and add them to Purchase History
            foreach (var cartItem in cart.CartItems)
            {
                var purchase = new Purchase
                {
                    BuyerId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    SellerId = cartItem.Item.CreatedBy, // Seller ID from the item
                    ItemId = cartItem.Item.Id,
                    Quantity = cartItem.Quantity,
                    PurchaseDate = DateTime.Now
                };

                _context.Purchase.Add(purchase);
            }

            // Clear the cart after checkout
            _cartService.ClearCart();

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Items");
        }

    }
}

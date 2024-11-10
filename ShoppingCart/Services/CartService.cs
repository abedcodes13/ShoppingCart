using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Cart GetCart()
        {
            // Get the currently logged-in user's ID
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return null; }

            // Retrieve the user's cart by ApplicationUserId
            var cart = _context.Cart.FirstOrDefault(c => c.ApplicationUserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId,
                    CartItems = new List<CartItem>()
                };
                _context.Cart.Add(cart);
                _context.SaveChanges();
            }

            // Include the cart items and related products
            cart.CartItems = _context.CartItem.Where(c => c.Cart.Id == cart.Id).Include(ci => ci.Item).ToList();
            return cart;
        }

        public void AddItem(Item product, int quantity)
        {
            Cart cart = GetCart();
            var item = cart.CartItems.FirstOrDefault(ci => ci.Item.Id == product.Id);

            if (item == null)
            {
                item = new CartItem { Item = product, Cart = cart, Quantity = quantity };
                cart.CartItems.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }

            _context.SaveChanges();
        }

        public int CartCount()
        {
            var cart = GetCart();
            return cart.CartItems.Sum(i => i.Quantity);
        }

        public int RemoveItem(int itemId, int qty = 1)
        {
            Cart cart = GetCart();
            var item = cart.CartItems.FirstOrDefault(ci => ci.Item.Id == itemId);

            if (item == null) return 0;

            item.Quantity -= qty;
            if (item.Quantity == 0)
            {
                cart.CartItems.Remove(item);
            }

            _context.SaveChanges();
            return item.Quantity;
        }

        public int TotalItem(int itemId)
        {
            Cart cart = GetCart();
            var item = cart.CartItems.FirstOrDefault(ci => ci.Item.Id == itemId);
            return item?.Quantity ?? 0;
        }

        public void ClearCart()
        {
            var cart = GetCart();
            cart.CartItems.Clear();
            _context.Cart.Remove(cart);
            _context.SaveChanges();
        }
    }
}

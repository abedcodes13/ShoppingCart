namespace ShoppingCart.Models
{
    public interface ICartService
    {
        void AddItem(Item item, int quantity);
        int RemoveItem(int itemId, int qty);
        int TotalItem(int itemId);
        int CartCount();
        void ClearCart();
        Cart GetCart();
    }
}

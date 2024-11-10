namespace ShoppingCart.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public int Quantity { get; set; }

        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }

        
        // Calculated property for item total price
        public decimal TotalPrice => Item.Price * Quantity;

    }
}

using ShoppingCart.Models;

public class Purchase
{
    public int Id { get; set; }
    public string BuyerId { get; set; } // Buyer User ID
    public ApplicationUser Buyer { get; set; } // Reference to the buyer

    public string SellerId { get; set; } // Seller User ID
    public ApplicationUser Seller { get; set; } // Reference to the seller

    public int ItemId { get; set; } // Purchased item
    public Item Item { get; set; }

    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; } // Date of the purchase
}

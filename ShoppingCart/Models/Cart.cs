using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class Cart
    {

        public int Id { get; set; }

        // Reference to the user who owns the cart
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        // A cart can have multiple items
        public ICollection<CartItem> CartItems { get; set; }


    }
}


//[Key]
//public string Username { get; set; }
//public Cart(string username)
//{
//    CartItems = new List<CartItem>();
//    Username = username;
//}
//public virtual IList<CartItem> CartItems { get; set; }
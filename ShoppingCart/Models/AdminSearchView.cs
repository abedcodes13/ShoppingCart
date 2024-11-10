using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingCart.Models
{
    public class AdminSearchView
    {
        public List<Item>? Items { get; set; }
        public SelectList? Categories { get; set; }
        public string? Category { get; set; }
        public string? SearchString { get; set; }
    }
}

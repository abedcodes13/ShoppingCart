namespace ShoppingCart.Models
{
    public class UserHomePageViewModel
    {
        public List<Item> ItemsForSale { get; set; }
        public List<CustomerReport> CustomersReport { get; set; }
    }

    public class CustomerReport
    {
        public string CustomerName { get; set; }
        public decimal TotalSpent { get; set; }
    }

}


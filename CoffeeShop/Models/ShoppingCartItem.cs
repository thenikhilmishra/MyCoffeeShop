namespace CoffeeShop.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public required Product Product { get; set; } = new Product { Name = string.Empty, Detail = string.Empty };
        public int Qty { get; set; }
        public string? ShoppingCartId { get; set; }
    }
}

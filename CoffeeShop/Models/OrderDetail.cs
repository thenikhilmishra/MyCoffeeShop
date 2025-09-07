namespace CoffeeShop.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; } = new Product { Name = string.Empty, Detail = string.Empty };
        public int OrderId { get; set; }
        public required Order Order { get; set; } = new Order { FirstName = string.Empty, LastName = string.Empty, Email = string.Empty, Phone = string.Empty, Address = string.Empty };
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

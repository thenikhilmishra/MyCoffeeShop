namespace CoffeeShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Phone { get; set; } = string.Empty;
        public required string Address { get; set; } = string.Empty;
        public decimal OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new();
        public string? UserId { get; set; }
        public string? Status { get; set; } // Add this for order status
    }
}

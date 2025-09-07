namespace CoffeeShop.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Message { get; set; } = string.Empty;
    }
}

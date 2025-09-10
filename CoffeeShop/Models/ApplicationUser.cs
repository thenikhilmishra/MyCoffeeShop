using Microsoft.AspNetCore.Identity;

namespace CoffeeShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Address { get; set; } = string.Empty;
        public DateTime LastLoginUtc { get; internal set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
    }
}

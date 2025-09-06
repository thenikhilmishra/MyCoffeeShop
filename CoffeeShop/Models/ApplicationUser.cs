using Microsoft.AspNetCore.Identity;

namespace CoffeeShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } // Optional
        public string Address { get; set; }
    }
}

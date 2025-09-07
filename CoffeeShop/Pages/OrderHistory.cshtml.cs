using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CoffeeShop.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly CoffeeShopDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderHistoryModel(CoffeeShopDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Order> Orders { get; set; } = new();
        public string? StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var email = user.Email?.ToLowerInvariant();
                Orders = await _context.Orders
                    .Where(o => o.Email == email)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                    .OrderByDescending(o => o.OrderPlaced)
                    .ToListAsync();
            }
            else
            {
                Orders = new List<Order>();
            }
        }
    }
}
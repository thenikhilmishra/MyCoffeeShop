using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly CoffeeShopDbContext _context;

        public OrderHistoryModel(CoffeeShopDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = User.Identity.Name; // Or use User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            Orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(o => o.OrderPlaced)
                .ToListAsync();
        }
    }
}
using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class OrdersModel : PageModel
    {
        private readonly CoffeeShopDbContext _context;

        public OrdersModel(CoffeeShopDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            Orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(o => o.OrderPlaced)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order != null)
            {
                _context.OrderDetails.RemoveRange(order.OrderDetails);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Data;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly CoffeeShopDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileModel(CoffeeShopDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public ApplicationUser? UserInfo { get; set; }
    public List<Order> Orders { get; set; } = new();
    public ContactMessage? ContactInfo { get; set; }
    public string? StatusMessage { get; set; }

    public async Task OnGetAsync()
    {
        UserInfo = await _userManager.GetUserAsync(User);
        if (UserInfo != null)
        {
            var email = UserInfo.Email?.ToLowerInvariant();
            Orders = await _context.Orders
                .Where(o => o.Email == email)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(o => o.OrderPlaced)
                .ToListAsync();

            ContactInfo = await _context.ContactMessages
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email);
        }
        else
        {
            Orders = new List<Order>();
            ContactInfo = null;
        }
    }
}
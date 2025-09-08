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
    public List<ContactMessage> ContactMessages { get; set; } = new();
    public string? StatusMessage { get; set; }

    public async Task OnGetAsync()
    {
        UserInfo = await _userManager.GetUserAsync(User);
        if (UserInfo != null)
        {
            var email = UserInfo.Email?.Trim().ToLowerInvariant();
            Orders = await _context.Orders
                .Where(o => EF.Functions.Like(o.Email, email))
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .OrderByDescending(o => o.OrderPlaced)
                .ToListAsync();

            ContactMessages = await _context.ContactMessages
                .Where(c => EF.Functions.Like(c.Email, email))
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();
        }
        else
        {
            Orders = new List<Order>();
            ContactMessages = new List<ContactMessage>();
        }
    }
}
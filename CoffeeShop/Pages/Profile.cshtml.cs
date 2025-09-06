using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly CoffeeShopDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProfileModel(CoffeeShopDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IdentityUser? UserInfo { get; set; }
    public List<Order> Orders { get; set; } = new();

    public async Task OnGetAsync()
    {
        UserInfo = await _userManager.GetUserAsync(User);
        if (UserInfo != null)
        {
            Orders = await _context.Orders
                .Where(o => o.UserId == UserInfo.Id)
                .OrderByDescending(o => o.OrderPlaced)
                .ToListAsync();
        }
        else
        {
            Orders = new List<Order>();
        }
    }
}
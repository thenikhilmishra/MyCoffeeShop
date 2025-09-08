using CoffeeShop.Models;
using CoffeeShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminProfileModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CoffeeShopDbContext _context;

    public AdminProfileModel(UserManager<ApplicationUser> userManager, CoffeeShopDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public ApplicationUser? AdminInfo { get; set; }
    public int TotalUsers { get; set; }
    public int UsersLoggedIn24h { get; set; }
    public int NewRegistrations24h { get; set; }
    public int NewContactMessages24h { get; set; }
    public int NewOrders24h { get; set; }
    public int TotalOrders { get; set; }
    public int TotalContactMessages { get; set; }
    public int TotalRegistrations { get; set; }

    public async Task OnGetAsync()
    {
        AdminInfo = await _userManager.GetUserAsync(User);
        var now = DateTime.UtcNow;
        var last24h = now.AddHours(-24);

        TotalUsers = await _userManager.Users.CountAsync();
        // Registration time not tracked, so show total confirmed users only
        NewRegistrations24h = 0;
        TotalRegistrations = await _userManager.Users.CountAsync(u => u.EmailConfirmed);

        UsersLoggedIn24h = await _userManager.Users.CountAsync(u => u.LastLoginUtc >= last24h);

        NewContactMessages24h = await _context.ContactMessages.CountAsync(m => m.SubmittedAt >= last24h);
        TotalContactMessages = await _context.ContactMessages.CountAsync();

        NewOrders24h = await _context.Orders.CountAsync(o => o.OrderPlaced >= last24h);
        TotalOrders = await _context.Orders.CountAsync();
    }
}
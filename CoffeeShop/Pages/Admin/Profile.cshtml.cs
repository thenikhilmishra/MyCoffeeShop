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
    public int TotalRegistrations { get; set; }
    public int TotalProducts { get; set; }
    public int TrendingProducts { get; set; }
    public int TotalOrders { get; set; }
    public int NewOrders24h { get; set; }
    public decimal Revenue24h { get; set; }
    public decimal TotalRevenue { get; set; }
    public int ApprovedOrders { get; set; }
    public int CancelledOrders { get; set; }
    public int PendingOrders { get; set; }
    public int TotalContactMessages { get; set; }
    public int NewContactMessages24h { get; set; }
    public int UnreadContactMessages { get; set; }
    public int RepliedContactMessages { get; set; }
    public List<Order> RecentOrders { get; set; } = new();
    public List<ContactMessage> RecentContacts { get; set; } = new();
    public List<Product> TrendingProductList { get; set; } = new();
    public int MaleUsers { get; set; }
    public int FemaleUsers { get; set; }
    public int OtherUsers { get; set; }
    public int CountryCount { get; set; }

    public async Task OnGetAsync()
    {
        AdminInfo = await _userManager.GetUserAsync(User);
        var now = DateTime.UtcNow;
        var last24h = now.AddHours(-24);

        TotalUsers = await _userManager.Users.CountAsync();
        TotalRegistrations = await _userManager.Users.CountAsync(u => u.EmailConfirmed);
        UsersLoggedIn24h = await _userManager.Users.CountAsync(u => u.LastLoginUtc >= last24h);

        TotalProducts = await _context.Products.CountAsync();
        TrendingProducts = await _context.Products.CountAsync(p => p.IsTrendingProduct);
        TrendingProductList = await _context.Products.Where(p => p.IsTrendingProduct).OrderByDescending(p => p.Id).Take(5).ToListAsync();

        TotalOrders = await _context.Orders.CountAsync();
        NewOrders24h = await _context.Orders.CountAsync(o => o.OrderPlaced >= last24h);
        TotalRevenue = await _context.Orders.SumAsync(o => o.OrderTotal);
        Revenue24h = await _context.Orders.Where(o => o.OrderPlaced >= last24h).SumAsync(o => o.OrderTotal);
        ApprovedOrders = await _context.Orders.CountAsync(o => o.Status == "Approved");
        CancelledOrders = await _context.Orders.CountAsync(o => o.Status == "Cancelled");
        PendingOrders = await _context.Orders.CountAsync(o => o.Status == null || o.Status == "Pending");
        RecentOrders = await _context.Orders.OrderByDescending(o => o.OrderPlaced).Take(5).Include(o => o.OrderDetails).ThenInclude(od => od.Product).ToListAsync();

        TotalContactMessages = await _context.ContactMessages.CountAsync();
        NewContactMessages24h = await _context.ContactMessages.CountAsync(m => m.SubmittedAt >= last24h);
        UnreadContactMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead);
        RepliedContactMessages = await _context.ContactMessages.CountAsync(m => m.AdminReply != null && m.AdminReply != "");
        RecentContacts = await _context.ContactMessages.OrderByDescending(m => m.SubmittedAt).Take(5).ToListAsync();

        MaleUsers = await _userManager.Users.CountAsync(u => u.Gender == "Male");
        FemaleUsers = await _userManager.Users.CountAsync(u => u.Gender == "Female");
        OtherUsers = await _userManager.Users.CountAsync(u => u.Gender == "Other");
        CountryCount = await _userManager.Users.Select(u => u.Country).Distinct().CountAsync();
    }
}
using CoffeeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "Admin")]
public class AdminProfileModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminProfileModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public ApplicationUser? AdminInfo { get; set; }
    public string? StatusMessage { get; set; }

    public async Task OnGetAsync()
    {
        AdminInfo = await _userManager.GetUserAsync(User);
    }
}
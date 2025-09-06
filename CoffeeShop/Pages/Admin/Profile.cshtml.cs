using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

[Authorize(Roles = "Admin")]
public class AdminProfileModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public AdminProfileModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public IdentityUser? AdminInfo { get; set; }

    public async Task OnGetAsync()
    {
        AdminInfo = await _userManager.GetUserAsync(User);
    }
}
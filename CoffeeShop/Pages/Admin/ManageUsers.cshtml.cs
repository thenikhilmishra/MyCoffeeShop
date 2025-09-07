using CoffeeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUsersModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public List<ApplicationUser> Users { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; } // Made nullable

        public async Task OnGetAsync()
        {
            Users = await Task.Run(() =>
                _userManager.Users
                    .Where(u => u.Email != "admin@coffeeshop.com") // Exclude admin
                    .ToList()
            );
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.Email != "admin@coffeeshop.com")
            {
                var result = await _userManager.DeleteAsync(user);
                StatusMessage = result.Succeeded ? "User deleted." : "Error deleting user.";
            }
            else
            {
                StatusMessage = "User not found or cannot delete admin.";
            }
            return RedirectToPage();
        }
    }
}

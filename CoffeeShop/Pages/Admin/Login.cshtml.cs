using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using CoffeeShop.Models; // Ensure this is included

namespace CoffeeShop.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required]
        public string Email { get; set; } = string.Empty;
        [BindProperty]
        [Required]
        public string Password { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string? StatusMessage { get; set; } // Made nullable

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please enter both email and password.";
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, Password))
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }

            // Check if user is in Admin role
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                ErrorMessage = "You are not authorized as an admin!";
                return Page(); // Show notification, do not redirect
            }

            // Sign in and redirect to admin area
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToPage("/Admin/ManageProducts");
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Pages.Admin
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required]
        public string Email { get; set; }
        [BindProperty]
        [Required]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please enter both email and password.";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Email, Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToPage("/Admin/ManageProducts");
            }
            ErrorMessage = "Invalid login attempt.";
            return Page();
        }
    }
}
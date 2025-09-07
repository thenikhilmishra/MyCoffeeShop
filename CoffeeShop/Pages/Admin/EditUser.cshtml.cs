using CoffeeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            public string Id { get; set; }
            [Required]
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                StatusMessage = "User not found.";
                return RedirectToPage("ManageUsers");
            }

            Input = new InputModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(Input.Id);
            if (user == null)
            {
                StatusMessage = "User not found.";
                return RedirectToPage("ManageUsers");
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Email = Input.Email;
            user.UserName = Input.Email;
            user.PhoneNumber = Input.PhoneNumber;
            user.Address = Input.Address;

            var result = await _userManager.UpdateAsync(user);
            StatusMessage = result.Succeeded ? "User updated." : "Error updating user.";
            return Page();
        }
    }
}

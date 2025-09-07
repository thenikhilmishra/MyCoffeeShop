using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Pages
{
    public class ContactModel : PageModel
    {
        private readonly CoffeeShopDbContext _context;
        public ContactModel(CoffeeShopDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ContactMessage Contact { get; set; } = new ContactMessage { Name = string.Empty, Email = string.Empty, Message = string.Empty };

        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Contact.SubmittedAt = DateTime.UtcNow;
            _context.ContactMessages.Add(Contact);
            await _context.SaveChangesAsync();
            SuccessMessage = "Thank you for contacting us! We will get back to you soon.";

            // Clear the form fields on the server side
            ModelState.Clear();
            Contact = new ContactMessage { Name = string.Empty, Email = string.Empty, Message = string.Empty };

            return Page();
        }
    }
}
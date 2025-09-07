using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ContactNotificationsModel : PageModel
    {
        private readonly CoffeeShopDbContext _context;

        public ContactNotificationsModel(CoffeeShopDbContext context)
        {
            _context = context;
        }

        public List<ContactMessage> Messages { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            Messages = await _context.ContactMessages
                .OrderByDescending(m => m.SubmittedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostReplyAsync(int id, string reply)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null)
            {
                msg.AdminReply = reply;
                msg.IsRead = true;
                await _context.SaveChangesAsync();
                StatusMessage = "Reply sent and message marked as read.";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMarkReadAsync(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null)
            {
                msg.IsRead = true;
                await _context.SaveChangesAsync();
                StatusMessage = "Message marked as read.";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var msg = await _context.ContactMessages.FindAsync(id);
            if (msg != null)
            {
                _context.ContactMessages.Remove(msg);
                await _context.SaveChangesAsync();
                StatusMessage = "Message deleted.";
            }
            return RedirectToPage();
        }
    }
}
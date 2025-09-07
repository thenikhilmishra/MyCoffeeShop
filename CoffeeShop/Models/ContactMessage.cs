using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public required string Name { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public required string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required, StringLength(1000)]
        public required string Message { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public string? AdminReply { get; set; }
    }
}
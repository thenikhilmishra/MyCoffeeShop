using System;
using System.ComponentModel.DataAnnotations;

public class ContactMessage
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [Required, StringLength(1000)]
    public string Message { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public bool IsRead { get; set; } = false;

    public string? AdminReply { get; set; }
}
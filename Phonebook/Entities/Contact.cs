using System.ComponentModel.DataAnnotations;

namespace Phonebook.Entities;

public class Contact
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = String.Empty;
    [Phone]
    [Required]
    public string PhoneNumber { get; set; } = String.Empty;
    [EmailAddress]
    [Required]
    public string Email { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

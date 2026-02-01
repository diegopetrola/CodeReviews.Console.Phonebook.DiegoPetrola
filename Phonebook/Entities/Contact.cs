namespace Phonebook.Models;

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

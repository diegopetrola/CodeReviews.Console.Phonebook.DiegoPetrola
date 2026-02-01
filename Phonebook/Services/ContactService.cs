using Microsoft.EntityFrameworkCore;
using Phonebook.Context;
using Phonebook.Entities;

namespace Phonebook.Services;

public class ContactService(PhonebookContext phonebookContext) : IContactService
{
    public async Task<List<Contact>> GetContacts(string filter = "")
    {
        var contacts = await phonebookContext.Contacts.Where(c =>
                c.Name.Contains(filter) || c.PhoneNumber.Contains(filter))
            .AsNoTracking()
            .ToListAsync();

        return contacts;
    }

    public async Task AddContact(Contact contact)
    {
        await phonebookContext.Contacts.AddAsync(contact);
        await phonebookContext.SaveChangesAsync();
    }

    public async Task RemoveContact(int Id)
    {
        var c = await phonebookContext.Contacts.FindAsync(Id) ?? throw new ArgumentException("Contact not found.");
        phonebookContext.Contacts.Remove(c);
        await phonebookContext.SaveChangesAsync();
    }
}

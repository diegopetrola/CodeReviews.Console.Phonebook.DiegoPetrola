using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Phonebook.Context;
using Phonebook.Entities;
using Phonebook.Utils;

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
        var errorMsg = Shared.ValidadeContact(contact);
        if (!errorMsg.IsNullOrEmpty())
            throw new ArgumentException(errorMsg);

        contact.CreatedAt = DateTime.Now;
        await phonebookContext.Contacts.AddAsync(contact);
        await phonebookContext.SaveChangesAsync();
    }

    public async Task RemoveContact(int Id)
    {
        var c = await phonebookContext.Contacts.FindAsync(Id) ?? throw new ArgumentException("Contact not found.");
        phonebookContext.Contacts.Remove(c);
        await phonebookContext.SaveChangesAsync();
    }

    public async Task UpdateContact(Contact contact)
    {
        var errorMsg = Shared.ValidadeContact(contact);
        if (!errorMsg.IsNullOrEmpty())
            throw new ArgumentException(errorMsg);

        var dbContact = phonebookContext.Contacts.Find(contact.Id) ?? throw new ArgumentException("Contact not found.");
        dbContact.Name = contact.Name;
        dbContact.Email = contact.Email;
        dbContact.PhoneNumber = contact.PhoneNumber;

        phonebookContext.Contacts.Update(dbContact);
        await phonebookContext.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
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
        var dbContact = phonebookContext.Contacts.Find(contact.Id) ?? throw new ArgumentException("Contact not found.");
        dbContact.PhoneNumber = contact.PhoneNumber;
        dbContact.Name = contact.Name;

        var phone = contact.PhoneNumber;
        if (Shared.ValidatePhoneNumber(phone, out phone))
        {
            contact.PhoneNumber = phone;
        }
        else
        {
            throw new ArgumentException("Phone number is invalid");
        }
        phonebookContext.Contacts.Update(dbContact);
        await phonebookContext.SaveChangesAsync();
    }
}

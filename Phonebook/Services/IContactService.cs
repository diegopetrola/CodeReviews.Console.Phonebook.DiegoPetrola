using Phonebook.Entities;

namespace Phonebook.Services
{
    public interface IContactService
    {
        Task AddContact(Contact contact);
        Task<List<Contact>> GetContacts(string filter = "");
        Task RemoveContact(int Id);
        Task UpdateContact(Contact contact);
    }
}
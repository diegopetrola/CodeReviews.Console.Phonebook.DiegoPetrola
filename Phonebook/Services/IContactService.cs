using Phonebook.Entities;

namespace Phonebook.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetContacts(string filter = "");
    }
}
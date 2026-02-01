using Phonebook.Context;
using Phonebook.Models;

namespace Phonebook.Utils;

public static class DatabaseSeeding
{
    private static readonly string _errorMsg =
        $"[{ColorHelper.error}]Error while seeding the database. The application might not work![/]";

    public static async Task CustomSeeding()
    {
        using var context = new PhonebookContext();
        if (context.Contacts.Any()) return;

        var contact = new Contact { Name = "John Doe", PhoneNumber = "123123" };
        try
        {
            await context.Contacts.AddRangeAsync(contact);
            await context.SaveChangesAsync();
        }
    }
}

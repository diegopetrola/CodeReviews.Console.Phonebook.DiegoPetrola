using Phonebook.Context;
using Phonebook.Entities;
using Spectre.Console;

namespace Phonebook.Utils;

public static class DatabaseSeeding
{
    private static readonly string _errorMsg =
        $"[{ColorHelper.error}]Error while seeding the database. The application might not work![/]";

    public static async Task CustomSeeding()
    {
        using var context = new PhonebookContext();
        if (context.Contacts.Any()) return;

        var contacts = new List<Contact>
        {
            new() { Name = "John Presley", PhoneNumber = "21 2121-2221" },
            new() { Name = "Elvis Doe", PhoneNumber = "21 3231-8659" },
            new() { Name = "Albert Darwin", PhoneNumber = "32 3233-4425" },
            new() { Name = "Charles Einstein", PhoneNumber = "99 9792-7386" },
            new() { Name = "Sean Sapolsky", PhoneNumber = "99 9222-2257" },
            new() { Name = "Robert Carrol", PhoneNumber = "31 9792-1836" },
            new() { Name = "Emmy Franklin", PhoneNumber = "55 8923-8646" },
            new() { Name = "Rosalind Noether", PhoneNumber = "17 9792-7386" },
            new() { Name = "Alex Curie", PhoneNumber = "88 9792-7386" },
            new() { Name = "Marie O'Connor", PhoneNumber = "66 6626-7476" },
        };

        try
        {
            await context.Contacts.AddRangeAsync(contacts);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"""
                {_errorMsg}
                {e.Message}
                """);
        }
    }
}

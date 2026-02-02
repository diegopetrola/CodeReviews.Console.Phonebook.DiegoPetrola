using Microsoft.Extensions.DependencyInjection;
using Phonebook.Context;
using Phonebook.Entities;
using Spectre.Console;

namespace Phonebook.Utils;

public static class DatabaseSeeding
{
    private static readonly string _errorMsg =
        $"[{ColorHelper.error}]Error while seeding the database. The application might not work![/]";

    public static async Task CustomSeeding(ServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<PhonebookContext>();
        if (context.Contacts.Any()) return;

        var contacts = new List<Contact>
        {
            new() { Name = "John Presley", PhoneNumber = "21 2121-2221", Email="johnp@email.com" },
            new() { Name = "Elvis Doe", PhoneNumber = "21 3231-8659", Email="delvis@email.com" },
            new() { Name = "Albert Darwin", PhoneNumber = "32 3233-4425", Email="albwin@email.com" },
            new() { Name = "Charles Einstein", PhoneNumber = "99 9792-7386", Email="chalsein@email.com" },
            new() { Name = "Sean Sapolsky", PhoneNumber = "99 9222-2257", Email="ssapo@email.com" },
            new() { Name = "Robert Carrol", PhoneNumber = "31 9792-1836", Email="robc@other.com" },
            new() { Name = "Emmy Franklin", PhoneNumber = "55 8923-8646", Email="emmy@email.com" },
            new() { Name = "Rosalind Noether", PhoneNumber = "17 9792-7386", Email="rosa@email.com" },
            new() { Name = "Alex Curie", PhoneNumber = "88 9792-7386", Email="alex@email.com" },
            new() { Name = "Marie O'Connor", PhoneNumber = "66 6626-7476", Email="marie@email.com" },
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

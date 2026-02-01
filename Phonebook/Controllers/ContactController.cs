using Microsoft.IdentityModel.Tokens;
using Phonebook.Entities;
using Phonebook.Services;
using Phonebook.Utils;
using Spectre.Console;
namespace Phonebook.Controllers;

public class ContactController(IContactService contactService)
{
    public async Task ShowMainMenu()
    {
        var filter = "";
        var exit = false;
        while (!exit)
        {
            var contacts = await contactService.GetContacts(filter);

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<Contact>()
                .Title("Choose a flashcard to show more:")
                .WrapAround(true)
                .AddChoices(new Contact { Name = Shared.addNew })
                .AddChoices(new Contact { Name = Shared.filter })
                .AddChoices(new Contact { Name = Shared.goBack })
                .AddChoices(contacts)
                .WrapAround(true)
                .PageSize(30)
                .UseConverter(c => $"{c.Name}" + (!c.PhoneNumber.IsNullOrEmpty() ? $" - {c.PhoneNumber}" : ""))
            );

            if (selection.Name.Equals(Shared.filter))
                filter = await FilterScreen();
            else if (selection.Name.Equals(Shared.addNew))
                await AddContactScreen();
            else if (selection.Name.Equals(Shared.goBack))
                exit = true;
        }
    }

    public async Task<string> FilterScreen()
    {
        var filter = AnsiConsole.Ask($"Type your filter for [{ColorHelper.bold}]phone[/] or [{ColorHelper.bold}]name[/].", "");
        return filter;
    }

    public async Task AddContactScreen()
    {

    }
}

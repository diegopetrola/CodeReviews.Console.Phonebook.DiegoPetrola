using Microsoft.IdentityModel.Tokens;
using Phonebook.Entities;
using Phonebook.Services;
using Phonebook.Utils;
using Spectre.Console;
namespace Phonebook.Controllers;

public class ContactController(IContactService contactService)
{
    private enum MenuOptions
    {
        Update,
        Delete,
        GoBack,
    };

    private static string MenuOptionsToString(MenuOptions option)
    {
        return option switch
        {
            MenuOptions.GoBack => "Go Back",
            _ => option.ToString()
        };
    }

    public async Task ShowMainMenu()
    {
        var filter = "";
        var exit = false;
        while (!exit)
        {
            var contacts = await contactService.GetContacts(filter);
            AnsiConsole.Clear();
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<Contact>()
                .Title("Choose a contact to show more:")
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
            else
                await ShowContactScreen(selection);
        }
    }

    public static async Task<string> FilterScreen()
    {
        var prompt = new TextPrompt<string>($"Type your filter for [{ColorHelper.bold}]name[/] or" +
                $" [{ColorHelper.bold}]phone[/] ([{ColorHelper.subtle}]leave empty to reset[/]).")
            .DefaultValue("")
            .ShowDefaultValue(false);
        var filter = AnsiConsole.Prompt(prompt);

        return filter;
    }

    public async Task AddContactScreen()
    {
        var name = AnsiConsole.Ask<string>($"Type the new [{ColorHelper.bold}]contact name[/]:");
        var phoneNumber = Shared.AskPhoneNumber($"Type the new [{ColorHelper.bold}]phone number[/]:");
        var contact = new Contact { PhoneNumber = phoneNumber, Name = name };

        try
        {
            await contactService.AddContact(contact);
            AnsiConsole.MarkupLine($"[{ColorHelper.success}]Contact added.[/]");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.error}]Database error[/]! Please check the error below.");
            AnsiConsole.MarkupLine($"{e.Message}");
        }
        Shared.AskForKey();
    }

    public async Task ShowContactScreen(Contact contact)
    {
        var panel = Shared.GetStandardPanel($"""
            [{ColorHelper.subtle}]Phone:   [/]{contact.PhoneNumber}
            [{ColorHelper.subtle}]Created: [/]{contact.CreatedAt:dd/MM/yyyy}
            """,
            contact.Name);

        AnsiConsole.Clear();
        AnsiConsole.Write(panel);

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
                .Title("What would you like to do?")
                .WrapAround(true)
                .AddChoices(Enum.GetValues<MenuOptions>())
                .UseConverter(MenuOptionsToString)
            );

        switch (selection)
        {
            case (MenuOptions.Update):
                await UpdateContactScreen(contact);
                break;
            case (MenuOptions.Delete):
                await DeleteContactScreen(contact);
                break;
            default:
                return;
        }
    }

    private async Task DeleteContactScreen(Contact contact)
    {
        var prompt = new SelectionPrompt<bool>()
            .Title($"[{ColorHelper.error}]Are you sure?[/]")
            .WrapAround(true)
            .UseConverter(o => o ? "Yes" : "No")
            .AddChoices([true, false]);

        if (AnsiConsole.Prompt(prompt))
        {
            try
            {
                await contactService.RemoveContact(contact.Id);
                AnsiConsole.MarkupLine($"[{ColorHelper.warning}]Contact removed.[/]");
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[{ColorHelper.error}]Database error[/]! Please check the error below.");
                AnsiConsole.MarkupLine($"{e.Message}");
            }
            Shared.AskForKey();
        }
    }

    private async Task UpdateContactScreen(Contact contact)
    {
        contact.Name = AnsiConsole.Ask($"Type the new [{ColorHelper.bold}]name[/]", contact.Name);
        contact.PhoneNumber = Shared.AskPhoneNumber(
            $"Type the new [{ColorHelper.bold}]phone number[/] [{ColorHelper.success}]({contact.PhoneNumber})[/]",
            contact.PhoneNumber
        );

        try
        {
            await contactService.UpdateContact(contact);
            AnsiConsole.MarkupLine($"[{ColorHelper.warning}]Contact updated.[/]");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.error}]Database error[/]! Please check the error below.");
            AnsiConsole.MarkupLine($"{e.Message}");
        }
        Shared.AskForKey();
    }
}

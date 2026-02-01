using Spectre.Console;

namespace Phonebook.Controllers;

public class MainMenuController(ContactController contactController)
{
    private enum MainMenuOptions
    {
        StartContact,
        Exit
    };
    private static string MenuOptionsToString(MainMenuOptions option)
    {
        return option switch
        {
            MainMenuOptions.StartContact => "Show Contacts",
            _ => option.ToString()
        };
    }
    public async Task ShowMainMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                    .Title("What would you like to do?")
                    .WrapAround(true)
                    .AddChoices(Enum.GetValues<MainMenuOptions>())
                    .UseConverter(MenuOptionsToString)
                );

            switch (selection)
            {
                case MainMenuOptions.StartContact:
                    await contactController.ShowMainMenu();
                    break;
                case MainMenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Phonebook.Controllers;

public class MainMenuController(IServiceScopeFactory scopeFactory)
{
    //Font credits: patorjk.com
    private readonly string MenuText = "[Orange1]\nooooooooo.   oooo                                         .o8                           oooo        \r\n`888   `Y88. `888                                        \"888                           `888        \r\n 888   .d88'  888 .oo.    .ooooo.  ooo. .oo.    .ooooo.   888oooo.   .ooooo.   .ooooo.   888  oooo  \r\n 888ooo88P'   888P\"Y88b  d88' `88b `888P\"Y88b  d88' `88b  d88' `88b d88' `88b d88' `88b  888 .8P'   \r\n 888          888   888  888   888  888   888  888ooo888  888   888 888   888 888   888  888888.    \r\n 888          888   888  888   888  888   888  888    .o  888   888 888   888 888   888  888 `88b.  \r\no888o        o888o o888o `Y8bod8P' o888o o888o `Y8bod8P'  `Y8bod8P' `Y8bod8P' `Y8bod8P' o888o o888o \r\n                                                                                                    \r\n                                                                                                    \r\n                                                                                                    [/]";

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
            AnsiConsole.MarkupLine(MenuText);
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                    .Title(" What would you like to do?")
                    .WrapAround(true)
                    .AddChoices(Enum.GetValues<MainMenuOptions>())
                    .UseConverter(MenuOptionsToString)
                );

            switch (selection)
            {
                case MainMenuOptions.StartContact:
                    using (var scope = scopeFactory.CreateScope())
                    {
                        var contactController = scope.ServiceProvider.GetRequiredService<ContactController>();
                        await contactController.ShowMainMenu();
                    }
                    break;
                case MainMenuOptions.Exit:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
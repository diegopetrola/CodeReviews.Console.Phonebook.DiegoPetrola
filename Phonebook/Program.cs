using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Phonebook.Context;
using Phonebook.Controllers;
using Phonebook.Services;
using Phonebook.Utils;

IServiceCollection services = new ServiceCollection();
services.AddDbContext<PhonebookContext>();
services.AddTransient<IContactService, ContactService>();
services.AddTransient<ContactController>();
services.AddTransient<MainMenuController>();

ServiceProvider serviceProvider = services.BuildServiceProvider();

await DatabaseSeeding.CustomSeeding();

var mainMenu = serviceProvider.GetService<MainMenuController>()!;
await mainMenu.ShowMainMenu();

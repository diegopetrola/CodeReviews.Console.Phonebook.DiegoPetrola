using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Phonebook.Context;

IServiceCollection services = new ServiceCollection();
services.AddDbContext<PhonebookContext>();
services.AddTransient<IFlashcardService, FlashcardService>();
services.AddTransient<ICardStackService, CardStackService>();
services.AddTransient<IStudyService, StudyService>();
services.AddTransient<FlashcardController>();
services.AddTransient<StackController>();
services.AddTransient<ReportController>();
services.AddTransient<StudyController>();
services.AddTransient<MainMenuController>();

ServiceProvider serviceProvider = services.BuildServiceProvider();

await DatabaseSeeding.CustomSeeding();

var mainMenu = serviceProvider.GetService<MainMenuController>()!;
await mainMenu.ShowMainMenu();

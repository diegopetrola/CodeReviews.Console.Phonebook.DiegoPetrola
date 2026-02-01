using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phonebook.Entities;
using Phonebook.Utils;
using Spectre.Console;

namespace Phonebook.Context;

public class PhonebookContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public PhonebookContext() : base() { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false)
              .Build();

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[{ColorHelper.error}]The appsettings.json was deleted or moved, the application can not run![/]");
            AnsiConsole.MarkupLine($"Cloning the repository again might solve this.\n");
            AnsiConsole.MarkupLine($"Original error:[{ColorHelper.error}]{e.Message}[/]");
            Shared.AskForKey();
            Environment.Exit(0);
        }
    }
}


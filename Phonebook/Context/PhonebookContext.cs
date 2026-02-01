using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phonebook.Models;

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
        catch
        {
            throw new Exception("The appsettings.json was deleted or moved, the application can not run.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.Property(fc => fc.Name).IsRequired();
            entity.Property(fc => fc.PhoneNumber).IsRequired();
        });

    }
}


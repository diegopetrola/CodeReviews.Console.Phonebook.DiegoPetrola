using Microsoft.IdentityModel.Tokens;
using Phonebook.Entities;
using Spectre.Console;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Phonebook.Utils;

public static partial class Shared
{
    public static readonly string addNew = $"[{ColorHelper.success}] + Add New[/]";
    public static readonly string filter = $"[{ColorHelper.warning}] ~ Filter[/]";
    public static readonly string goBack = $"[{ColorHelper.subtle}]<- Go Back[/]";
    public static readonly string dateFormat = "dd/MM/yy";
    [GeneratedRegex("[0-9]{2} [0-9]{4}-[0-9]{4}")]
    private static partial Regex FormattedPhoneRegex();
    [GeneratedRegex("[0-9]{10}")]
    private static partial Regex Lenght10Regex();
    [GeneratedRegex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
    private static partial Regex MailRegex();

    public static Panel GetStandardPanel(string bodyText, string header)
    {
        var panel = new Panel(bodyText);
        panel.Header = new PanelHeader(header).Centered();
        panel.Border = BoxBorder.Rounded;
        panel.BorderColor(Color.Orange1);
        panel.Padding(5, 1);
        return panel;
    }

    public static void AskForKey(string message = "\nPress any key to continue...")
    {
        AnsiConsole.MarkupLine($"\n[{ColorHelper.subtle}]{message}[/]");
        Console.ReadKey(true);
    }

    public static bool ValidateStringDate(string dateString, out DateTime date)
    {
        var isValid = DateTime.TryParseExact(
                    dateString,
                    dateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out date);

        return isValid;
    }

    public static DateTime AskDate(string message)
    {
        var date = DateTime.Now;
        AnsiConsole.Prompt(new TextPrompt<string>(message)
            .Validate(input =>
                ValidateStringDate(input, out date) ?
                ValidationResult.Success() : ValidationResult.Error("Invalid date!")));

        return date;
    }

    public static string AskPhoneNumber(string message, string defaultValue = "")
    {
        var phone = defaultValue;
        AnsiConsole.Prompt(new TextPrompt<string>(message)
            .AllowEmpty()
            .Validate(input =>
                ValidatePhoneNumber(input.IsNullOrEmpty() ? phone : input, out phone) ?
                ValidationResult.Success() :
                ValidationResult.Error($"Invalid phone number! " +
                $"You can type 10 digits or something like [{ColorHelper.bold}]12 1234-1234[/]")));

        return phone;
    }

    public static bool ValidatePhoneNumber(string input, out string phone)
    {
        phone = input;
        if (Lenght10Regex().IsMatch(input))
        {
            phone = $"{phone[..2]} {phone[2..6]}-{phone[6..]}";
            return true;
        }
        else if (FormattedPhoneRegex().IsMatch(input))
            return true;

        return false;
    }

    public static bool ValidadeEMail(string email)
    {
        return !email.IsNullOrEmpty() && MailRegex().IsMatch(email);
    }

    public static string AskEmail(string defaultValue = "")
    {
        var prompt = new TextPrompt<string>($"Type the new [{ColorHelper.bold}]e-mail[/]")
                        .Validate(input => ValidadeEMail(input) ?
                            ValidationResult.Success() :
                            ValidationResult.Error("Invalid e-mail.")
                        );
        if (!defaultValue.IsNullOrEmpty())
            prompt.DefaultValue(defaultValue);
        var mail = AnsiConsole.Prompt(prompt);

        return mail;
    }

    public static string ValidadeContact(Contact contact)
    {
        var errorMsg = "";
        var phone = contact.PhoneNumber;
        if (!ValidatePhoneNumber(phone, out phone))
        {
            errorMsg += "Phone number is invalid\n";
        }

        if (!ValidadeEMail(contact.Email))
        {
            errorMsg += "Email is invalid";
        }

        return errorMsg;
    }
}

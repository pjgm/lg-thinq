using Spectre.Console;
using ThinQ.CLI.Configuration;

namespace ThinQ.CLI.Ui;

public static class ConsoleInputReader
{
    public static UserConfig ReadUserConfig(Guid clientId)
    {
        var username = ReadUsername();
        var password = ReadPassword();
        var countryCode = ReadCountryCode();
        var languageCode = ReadLanguageCode();

        return new UserConfig(username, password, languageCode, countryCode, clientId.ToString());
    }

    public static string ReadUsername()
    {
        return AnsiConsole.Ask<string>("Enter your LG account [green]username[/]:");
    }

    public static string ReadPassword()
    {
        return AnsiConsole.Ask<string>("Enter your LG account [red]password[/]:");
    }

    public static string ReadCountryCode()
    {
        return AnsiConsole.Ask<string>("Enter your LG account [yellow]country code[/]:");
    }

    public static string ReadLanguageCode()
    {
        return AnsiConsole.Ask<string>("Enter your LG account [yellow]language code[/]:");
    }
}

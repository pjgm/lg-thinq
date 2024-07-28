using Spectre.Console;
using ThinQ.Configuration;

namespace ThinQ.Cli.Ui;

public class ConsoleInputReader : IUserConfigReader
{
    public string ReadUsername() => AnsiConsole.Ask<string>("Enter your LG account [green]username[/]:");

    public string ReadPassword() => AnsiConsole.Ask<string>("Enter your LG account [red]password[/]:");

    public string ReadCountryCode() => AnsiConsole.Ask<string>("Enter your LG account [yellow]country code[/]:");

    public string ReadLanguageCode() => AnsiConsole.Ask<string>("Enter your LG account [yellow]language code[/]:");
}

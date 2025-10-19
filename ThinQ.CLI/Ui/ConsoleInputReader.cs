using Spectre.Console;
using ThinQ.Configuration;

namespace ThinQ.Cli.Ui;

public class ConsoleInputReader : IUserConfigReader
{
    public string ReadPersonalAccessToken() => AnsiConsole.Ask<string>("Enter your LG account [red]Personal Access Token[/]:");

    public string ReadCountryCode() => AnsiConsole.Ask<string>("Enter your LG account [yellow]country code[/]:");
}

using Spectre.Console;
using ThinQ.Mqtt;

namespace ThinQ.Cli.Ui;

public class ConsoleOutputWriter : IMqttOutputWriter
{
    public void WriteMessageReceived(string message) => AnsiConsole.MarkupLine($"[yellow]Received message[/]: {message}");

    public void WriteOnConnect() => AnsiConsole.MarkupLine("[green]Connected[/]");

    public void WriteOnDisconnect() => AnsiConsole.MarkupLine("[red]Disconnected[/]");

    public void WriteSubscribeToTopic(string topic) => AnsiConsole.MarkupLine($"[yellow]Subscribing to topic[/]: {topic}");
}

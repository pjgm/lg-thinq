using Spectre.Console;
using ThinQ.Cli.Ui;
using ThinQ.HttpClients;
using ThinQ.Mqtt;
using ThinQ.SessionManagement;

var sessionManager = new SessionManager(new ConsoleInputReader());
var session = await sessionManager.GetOrCreate();

var thinQClient = new ThinQClient(session);

var devices = await thinQClient.GetDevices();

var mqttManager = new MqttManager(thinQClient, new ConsoleOutputWriter(), session.ClientId);
await mqttManager.Connect();

var deviceAlias = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Choose an AC unit")
        .AddChoices(devices.result.item.Select(x => x.alias).ToList()));

var selectedDeviceId = devices.result.item.First(x => x.alias == deviceAlias);

var action = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title($"Operation to execute on {deviceAlias}")
        .AddChoices(["Turn on", "Turn off"]));

switch (action)
{
    case "Turn on":
        await thinQClient.TurnOnAc(selectedDeviceId.deviceId);
        break;
    case "Turn off":
        await thinQClient.TurnOffAc(selectedDeviceId.deviceId);
        break;
}

Console.ReadLine();

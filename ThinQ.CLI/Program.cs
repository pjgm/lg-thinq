using Spectre.Console;
using ThinQ.CLI.HttpClients;
using ThinQ.CLI.Mqtt;
using ThinQ.CLI.SessionManagement;

var session = await SessionManager.GetOrCreate();

var thinQClient = new ThinQClient(session);

var devices = await thinQClient.GetDevices();

var mqttManager = new MqttManager(thinQClient, session.ClientId);
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

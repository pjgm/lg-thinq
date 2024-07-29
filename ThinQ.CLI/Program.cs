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

while (true)
{
    var deviceAlias = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose an AC unit")
            .AddChoices(devices.result.item.Select(x => x.alias).ToList()));

    var selectedDeviceId = devices.result.item.First(x => x.alias == deviceAlias);

    var action = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title($"Operation to execute on {deviceAlias}")
            .AddChoices(["Turn on", "Turn off", "Set temperature", "Set fan speed", "Enable jet mode", "Disable jet mode", "Set vertical step", "Set horizontal step"]));

    switch (action)
    {
        case "Turn on":
            await thinQClient.TurnOnAc(selectedDeviceId.deviceId);
            break;
        case "Turn off":
            await thinQClient.TurnOffAc(selectedDeviceId.deviceId);
            break;
        case "Set temperature":
            var temp = AnsiConsole.Ask<int>("Enter the desired temperature:");
            await thinQClient.SetTemperature(selectedDeviceId.deviceId, temp);
            break;
        case "Set fan speed":
            var speed = AnsiConsole.Ask<FanSpeed>("Enter the desired fan speed:");
            await thinQClient.SetFanSpeed(selectedDeviceId.deviceId, speed);
            break;
        case "Enable jet mode":
            await thinQClient.EnableJetMode(selectedDeviceId.deviceId);
            break;
        case "Disable jet mode":
            await thinQClient.DisableJetMode(selectedDeviceId.deviceId);
            break;
        case "Set vertical step":
            var verticalStep = AnsiConsole.Ask<VerticalStep>("Vertical step to set:");
            await thinQClient.SetVerticalStep(selectedDeviceId.deviceId, verticalStep);
            break;
        case "Set horizontal step":
            var horizontalStep = AnsiConsole.Ask<HorizontalStep>("Horizontal step to set:");
            await thinQClient.SetHorizontalStep(selectedDeviceId.deviceId, horizontalStep);
            break;
    }
}

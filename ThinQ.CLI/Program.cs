using ThinQ;
using ThinQ.Cli.Ui;
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
    AcConsoleInterface.RenderDeviceInfo(devices.result);
    var (selectedDeviceId, deviceAlias) = AcConsoleInterface.SelectUnit(devices.result);
    var operation = AcConsoleInterface.SelectOperation(deviceAlias);

    await thinQClient.ExecuteAcOperation(selectedDeviceId, operation);
}

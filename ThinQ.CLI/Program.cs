using ThinQ;
using ThinQ.Cli.Ui;
using ThinQ.SessionManagement;

var sessionManager = new SessionManager(new ConsoleInputReader());
var session = await sessionManager.GetOrCreate();

var thinQClient = new ThinQClient(session);

//var response = await httpClient.GetAsync($"devices/{officeAcId}/profile");

var devices = await thinQClient.GetDevices();

//var mqttManager = new MqttManager(thinQClient, new ConsoleOutputWriter(), session.ClientId);
//await mqttManager.Connect();

while (true)
{ 
    AcConsoleInterface.RenderDeviceInfo(devices.Response);
    var (selectedDeviceId, deviceAlias) = AcConsoleInterface.SelectUnit(devices.Response);
    var deviceProfile = await thinQClient.GetDeviceProfile(selectedDeviceId);
    var deviceStatus = await thinQClient.GetDeviceState(selectedDeviceId);
    await thinQClient.TurnOnAc(selectedDeviceId);
    
    
    
    //var operation = AcConsoleInterface.SelectOperation(deviceAlias);

    //await thinQClient.ExecuteAcOperation(selectedDeviceId, operation);
}

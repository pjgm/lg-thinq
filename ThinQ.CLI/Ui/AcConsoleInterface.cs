using System.Globalization;
using Spectre.Console;
using ThinQ.Models;

namespace ThinQ.Cli.Ui;

internal static class AcConsoleInterface
{
    internal static void RenderDeviceInfo(Device[] deviceList)
    {
        var table = new Table
        {
            Border = TableBorder.Heavy
        };

        table.AddColumn("Name");
        table.AddColumn("Id");
        //table.AddColumn("Status");
        //table.AddColumn("Current temperature");
        //table.AddColumn("Target temperature");

        foreach (var device in deviceList)
        {
            table.AddRow(
                device.DeviceInfo.Alias,
                device.DeviceId);
            //device.snapshot.airState_operation.ToString(CultureInfo.InvariantCulture) == "1" ? "On" : "Off",
            //device.snapshot.airState_tempState_current.ToString(CultureInfo.InvariantCulture),
            //device.snapshot.airState_tempState_target.ToString(CultureInfo.InvariantCulture));
        }
        AnsiConsole.Write(table);
    }

    internal static (string deviceId, string deviceAlias) SelectUnit(Device[] devices)
    {
        var deviceAlias =  AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an AC unit")
                .AddChoices(devices.Select(x => x.DeviceInfo.Alias).ToList()));

        return (devices.First(x => x.DeviceInfo.Alias == deviceAlias).DeviceId, deviceAlias);
    }

    internal static Operation SelectOperation(string deviceAlias)
    {
        var operationName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Operation to execute on {deviceAlias}")
                .AddChoices(Operation.AllOperations));

        return operationName switch
        {
            Operation.TurnOnFriendlyName => Operation.TurnOnAc,
            Operation.TurnOffFriendlyName => Operation.TurnOffAc,
            Operation.SetTemperatureFriendlyName => Operation.SetAcTemperature(AnsiConsole.Ask<int>("Enter the desired temperature:")),
            Operation.SetFanSpeedFriendlyName => Operation.SetAcFanSpeed(AnsiConsole.Ask<FanSpeed>("Enter the desired fan speed:")),
            Operation.EnableJetModeFriendlyName => Operation.EnableAcJetMode,
            Operation.DisableJetModeFriendlyName => Operation.DisableAcJetMode,
            Operation.SetVerticalStepFriendlyName => Operation.SetAcVerticalStep(AnsiConsole.Ask<VerticalStep>("Vertical step to set:")),
            Operation.SetHorizontalStepFriendlyName => Operation.SetAcHorizontalStep(AnsiConsole.Ask<HorizontalStep>("Horizontal step to set:")),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}


internal static class OperationName
{
    public static readonly string[] AllOperations = [
        TurnOn, TurnOff, SetTemperature, SetFanSpeed, EnableJetMode, DisableJetMode, SetVerticalStep, SetHorizontalStep
    ];

    public const string TurnOn = "Turn On";
    public const string TurnOff = "Turn off";
    public const string SetTemperature = "Set temperature";
    public const string SetFanSpeed = "Set fan speed";
    public const string EnableJetMode = "Enable jet mode";
    public const string DisableJetMode = "Disable jet mode";
    public const string SetVerticalStep = "Set vertical step";
    public const string SetHorizontalStep = "Set horizontal step";

}

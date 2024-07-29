namespace ThinQ;

public class Operation
{
    public static readonly string[] AllOperations =
    [
        TurnOnFriendlyName, TurnOffFriendlyName, SetTemperatureFriendlyName, SetFanSpeedFriendlyName,
        EnableJetModeFriendlyName, DisableJetModeFriendlyName, SetVerticalStepFriendlyName,
        SetHorizontalStepFriendlyName
    ];

    public const string TurnOnFriendlyName = "Turn On AC";
    public const string TurnOffFriendlyName = "Turn off AC";
    public const string SetTemperatureFriendlyName = "Set AC temperature";
    public const string SetFanSpeedFriendlyName = "Set AC fan speed";
    public const string EnableJetModeFriendlyName = "Enable AC jet mode";
    public const string DisableJetModeFriendlyName = "Disable AC jet mode";
    public const string SetVerticalStepFriendlyName = "Set AC vertical step";
    public const string SetHorizontalStepFriendlyName = "Set AC horizontal step";

    private Operation(string friendlyName, string dataKey, string dataValue)
    {
        FriendlyName = friendlyName;
        DataKey = dataKey;
        DataValue = dataValue;
    }

    public string FriendlyName { get; private set; }
    public string DataKey { get; private set; }
    public string DataValue { get; private set; }

    public static Operation TurnOnAc => new(TurnOnFriendlyName, "airState.operation", "1");
    public static Operation TurnOffAc => new(TurnOffFriendlyName, "airState.operation", "0");
    public static Operation SetAcTemperature(int temp) => new(SetTemperatureFriendlyName, "airState.tempState.target", temp.ToString());
    public static Operation SetAcFanSpeed(FanSpeed speed) => new(SetFanSpeedFriendlyName, "airState.windStrength", speed.ToString("D"));
    public static Operation EnableAcJetMode => new(EnableJetModeFriendlyName, "airState.wMode.jet", "1");
    public static Operation DisableAcJetMode => new(DisableJetModeFriendlyName, "airState.wMode.jet", "0");
    public static Operation SetAcVerticalStep(VerticalStep step) => new(SetVerticalStepFriendlyName, "airState.wDir.vStep", step.ToString("D"));
    public static Operation SetAcHorizontalStep(HorizontalStep step) => new(SetHorizontalStepFriendlyName, "airState.wDir.hStep", step.ToString("D"));
}


public enum VerticalStep
{
    Off,
    Highest,
    UpperHigh,
    LowerHigh,
    UpperLow,
    LowerLow,
    Lowest,
    Swing = 100
}

public enum HorizontalStep
{
    Off,
    Left,
    CenterLeft,
    Center,
    CenterRight,
    Right,
    LeftHalf = 13,
    RightHalf = 35,
    Swing = 100
}

public enum FanSpeed
{
    Low = 2,
    Medium = 4,
    High = 6,
    Auto = 8
}

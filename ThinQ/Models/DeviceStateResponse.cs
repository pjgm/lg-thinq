namespace ThinQ.Models;

public class DeviceStateResponse : BaseApiResponse
{
    public required DeviceState Response { get; set; }
}

public class DeviceState
{
    public RunStateInfo? RunState { get; set; }
    public AirConJobModeInfo? AirConJobMode { get; set; }
    public PowerSaveInfo? PowerSave { get; set; }
    public TemperatureInfo? Temperature { get; set; }
    public TemperatureInfo[]? TemperatureInUnits { get; set; }
    public AirFlowInfo? AirFlow { get; set; }
    public WindDirectionInfo? WindDirection { get; set; }
    public OperationInfo? Operation { get; set; }
    public TimerInfo? Timer { get; set; }
    public SleepTimerInfo? SleepTimer { get; set; }
}

public record RunStateInfo(RunState CurrentState);
public record AirConJobModeInfo(AirConJobMode CurrentJobMode);
public record PowerSaveInfo(bool PowerSaveEnabled);
public record TemperatureInfo(double CurrentTemperature, double TargetTemperature, string Unit);
public record AirFlowInfo(WindStrength WindStrength, WindStrengthDetail WindStrengthDetail);
public record WindDirectionInfo(bool RotateLeftRight, bool RotateUpDown);
public record OperationInfo(AirConOperationMode AirConOperationMode);

// TODO: figure out what values are possible here
public record TimerInfo(string RelativeStartTimer, int RelativeHourToStart, int RelativeMinuteToStart, string RelativeStopTimer);
public record SleepTimerInfo(string RelativeStopTimer);

public enum RunState
{
    NORMAL // This is not in the docs for AC, but seen in practice
}

public enum AirConJobMode
{
    AIR_DRY,
    COOL,
    AIR_CLEAN
}

public enum WindStrength
{
    LOW,
    MID,
    HIGH,
    AUTO
}

public enum WindStrengthDetail
{
    NATURE // this is nowhere in the docs for any device, wtf LG
}

public enum AirConOperationMode
{
    POWER_OFF,
    POWER_ON
}

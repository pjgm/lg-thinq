namespace ThinQ.Models;

public class GetDevicesResponse : BaseApiResponse
{
    public required Device[] Response { get; set; }
}


public class Device
{
    public required string DeviceId { get; set; }
    public required DeviceInfo DeviceInfo { get; set; }
}

public class DeviceInfo
{
    public required string DeviceType { get; set; }
    public required string ModelName { get; set; }
    public required string Alias { get; set; }
    public required bool Reportable { get; set; }
}
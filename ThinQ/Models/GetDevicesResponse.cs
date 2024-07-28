namespace ThinQ.Models;


public class GetDevicesResponse
{
    public string resultCode { get; set; }
    public DevicesResult result { get; set; }
}

public class DevicesResult
{
    public string langPackCommonVer { get; set; }
    public string langPackCommonUri { get; set; }
    public Item[] item { get; set; }
    public object[] group { get; set; }
}

public class Item
{
    public string appType { get; set; }
    public string modelAppType { get; set; }
    public string modelCountryCode { get; set; }
    public string countryCode { get; set; }
    public string modelName { get; set; }
    public int deviceType { get; set; }
    public string deviceCode { get; set; }
    public string alias { get; set; }
    public string deviceId { get; set; }
    public string fwVer { get; set; }
    public string imageFileName { get; set; }
    public string imageUrl { get; set; }
    public string smallImageUrl { get; set; }
    public string ssid { get; set; }
    public string softapId { get; set; }
    public string softapPass { get; set; }
    public string macAddress { get; set; }
    public string networkType { get; set; }
    public string timezoneCode { get; set; }
    public string timezoneCodeAlias { get; set; }
    public int utcOffset { get; set; }
    public string utcOffsetDisplay { get; set; }
    public int dstOffset { get; set; }
    public string dstOffsetDisplay { get; set; }
    public int curOffset { get; set; }
    public string curOffsetDisplay { get; set; }
    public string sdsGuide { get; set; }
    public string newRegYn { get; set; }
    public string remoteControlType { get; set; }
    public string userNo { get; set; }
    public string tftYn { get; set; }
    public double modelJsonVer { get; set; }
    public string modelJsonUri { get; set; }
    public double appModuleVer { get; set; }
    public string appModuleUri { get; set; }
    public string appRestartYn { get; set; }
    public int appModuleSize { get; set; }
    public double langPackProductTypeVer { get; set; }
    public string langPackProductTypeUri { get; set; }
    public string deviceState { get; set; }
    public Snapshot snapshot { get; set; }
    public bool online { get; set; }
    public string platformType { get; set; }
    public int area { get; set; }
    public double regDt { get; set; }
    public string blackboxYn { get; set; }
    public string modelProtocol { get; set; }
    public int order { get; set; }
    public string drServiceYn { get; set; }
    public FwInfoList[] fwInfoList { get; set; }
    public ModemInfo modemInfo { get; set; }
    public string guideTypeYn { get; set; }
    public string guideType { get; set; }
    public string regDtUtc { get; set; }
    public int regIndex { get; set; }
    public string groupableYn { get; set; }
    public string controllableYn { get; set; }
    public string combinedProductYn { get; set; }
    public string masterYn { get; set; }
    public string pccModelYn { get; set; }
    public SdsPid sdsPid { get; set; }
    public string autoOrderYn { get; set; }
    public bool initDevice { get; set; }
    public string existsEntryPopup { get; set; }
    public string matterYn { get; set; }
    public int tclcount { get; set; }
}

public class Snapshot
{
    public double airState_windStrength { get; set; }
    public double airState_wMode_lowHeating { get; set; }
    public double airState_diagCode { get; set; }
    public double airState_lightingState_displayControl { get; set; }
    public double airState_scan_space_layoutEstimationEdgeAngle { get; set; }
    public double airState_wDir_hStep { get; set; }
    public double mid { get; set; }
    public double airState_energy_onCurrent { get; set; }
    public double airState_wMode_airClean { get; set; }
    public double airState_quality_sensorMon { get; set; }
    public double airState_memory_query { get; set; }
    public double airState_quality_odor { get; set; }
    public double airState_tempState_target { get; set; }
    public double airState_miscFuncState_autoDryRemainTime { get; set; }
    public double airState_operation { get; set; }
    public double airState_airCare_lightingState_moodOnOff { get; set; }
    public double airState_wMode_jet { get; set; }
    public double airState_wDir_vStep { get; set; }
    public double timestamp { get; set; }
    public double airState_powerSave_basic { get; set; }
    public FwUpgradeInfo fwUpgradeInfo { get; set; }
    //public Static static { get; set; }
    public double airState_tempState_current { get; set; }
    public double airState_miscFuncState_extraOp { get; set; }
    public double airState_reservation_sleepTime { get; set; }
    public double airState_miscFuncState_autoDry { get; set; }
    public double airState_reservation_targetTimeToStart { get; set; }
    public Meta meta { get; set; }
    public bool online { get; set; }
    public double airState_opMode { get; set; }
    public double airState_reservation_targetTimeToStop { get; set; }
    public double airState_filterMngStates_maxTime { get; set; }
    public double airState_filterMngStates_useTime { get; set; }
}

public class FwUpgradeInfo
{
    public UpgSched upgSched { get; set; }
}

public class UpgSched
{
    public string upgUtc { get; set; }
    public string cmd { get; set; }
}

public class Static
{
    public string deviceType { get; set; }
    public string countryCode { get; set; }
}

public class Meta
{
    public bool allDeviceInfoUpdate { get; set; }
    public string messageId { get; set; }
}

public class FwInfoList
{
    public string checksum { get; set; }
    public double order { get; set; }
    public string partNumber { get; set; }
}

public class ModemInfo
{
    public string appVersion { get; set; }
    public string modelName { get; set; }
    public string modemType { get; set; }
    public string oneshot { get; set; }
    public string ruleEngine { get; set; }
    public double size { get; set; }
}

public class SdsPid
{
    public string sds4 { get; set; }
    public string sds3 { get; set; }
    public string sds2 { get; set; }
    public string sds1 { get; set; }
}

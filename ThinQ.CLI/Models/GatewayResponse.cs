namespace ThinQ.CLI.Models;

public class GatewayResponse
{
    public Result Result { get; set; }
}

public class Result
{
    public string EmpSpxUri { get; set; } = string.Empty;
    public string EmpTermsUri { get; set; } = string.Empty;
    public string Thinq2Uri { get; set; } = string.Empty;
}

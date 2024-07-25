using System.Text.Json.Serialization;

namespace ThinQ.CLI.Models;

public class PreLoginResponse
{
    [JsonPropertyName("encrypted_pw")]
    public string EncryptedPassword { get; set; } = String.Empty;

    [JsonPropertyName("signature")]
    public string Signature { get; set; } = String.Empty;

    [JsonPropertyName("tStamp")]
    public long TimeStamp { get; set; }

    public void Deconstruct(out string signature, out long timestamp, out string encryptedPassword)
    {
        signature = Signature;
        timestamp = TimeStamp;
        encryptedPassword = EncryptedPassword;
    }
}

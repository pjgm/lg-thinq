namespace ThinQ.Configuration;

public record UserConfig(
    string Username,
    string Password,
    string CountryCode,
    string LanguageCode,
    string ClientId);

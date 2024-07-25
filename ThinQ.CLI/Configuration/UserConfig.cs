namespace ThinQ.CLI.Configuration;

public record UserConfig(
    string Username,
    string Password,
    string CountryCode,
    string LanguageCode,
    string ClientId);

namespace ThinQ.Configuration;

public record UserConfig(
    string CountryCode, // ISO 3166-1 alpha-2
    string PersonalAccessToken);

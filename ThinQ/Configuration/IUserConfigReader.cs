namespace ThinQ.Configuration;

public interface IUserConfigReader
{
    string ReadUsername();
    string ReadPassword();
    string ReadCountryCode();
    string ReadLanguageCode();
}

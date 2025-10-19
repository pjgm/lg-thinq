namespace ThinQ.Configuration;

public interface IUserConfigReader
{
    string ReadPersonalAccessToken();
    string ReadCountryCode();
}

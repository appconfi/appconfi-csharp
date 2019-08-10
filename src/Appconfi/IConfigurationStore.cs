namespace Appconfi
{
    public interface IConfigurationStore
    {
        string GetVersion();

        ApplicationConfiguration GetConfiguration();

        bool IsUserTarget(string featureToggle, User user);
    }
}
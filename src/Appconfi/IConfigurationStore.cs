namespace Appconfi
{

    public interface IConfigurationStore
    {
        string GetVersion();

        ApplicationConfiguration GetConfiguration();
    }
}
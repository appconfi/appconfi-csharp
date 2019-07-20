namespace Appconfi
{
    using System.Threading.Tasks;

    public interface IConfigurationStore
    {
        Task<string> GetVersionAsync();

        Task<ApplicationConfiguration> GetConfigurationAsync();
    }
}
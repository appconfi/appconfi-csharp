namespace Appconfi
{
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class ConfigurationStore : IConfigurationStore
    {
        private readonly AppconfiClient client;

        public ConfigurationStore(AppconfiClient client)
        {
            this.client = client;
        }

        public async Task<ApplicationConfiguration> GetConfigurationAsync()
        {
            var resource = "api/v1/configurations";
            var request = client.PrepareRequest(resource);

            var result = await client.ExecuteAsync(request);

            var config = JsonConvert.DeserializeObject<ApplicationConfiguration>(result);

            return config;
        }

        public async Task<string> GetVersionAsync()
        {
            var resource = "api/v1/configurations/version";
            var request = client.PrepareRequest(resource);

            var result = await client.ExecuteAsync(request);

            return result;
        }
    }
}

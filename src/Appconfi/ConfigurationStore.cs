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

        public ApplicationConfiguration GetConfiguration()
        {
            var resource = "api/v1/configurations";
            var request = client.PrepareRequest(resource);

            var result = client.Execute(request);

            var config = JsonConvert.DeserializeObject<ApplicationConfiguration>(result);

            return config;
        }

        public string GetVersion()
        {
            var resource = "api/v1/configurations/version";
            var request = client.PrepareRequest(resource);

            var result = client.Execute(request);

            return result;
        }
    }
}

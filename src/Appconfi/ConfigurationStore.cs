namespace Appconfi
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

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
            var config = DeserializeConfiguration(result);

            return config;
        }

        public bool IsUserTarget(string featureToggle, User user)
        {
            var resource = "api/v1/user_targeting";
            var request = client.PrepareRequest(resource);

            request = $"{request}&toggle={featureToggle}&user={user.ToString()}";

            var result = client.Execute(request);
            return FeatureToggle.IsEnabled(result);
        }

        ApplicationConfiguration DeserializeConfiguration(string json)
        {
            var contract = new ApplicationConfiguration();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var ser = new DataContractJsonSerializer(contract.GetType(), new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true
                });
                contract = ser.ReadObject(ms) as ApplicationConfiguration;
            }

            return contract;
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

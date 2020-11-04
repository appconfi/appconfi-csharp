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
            var resource = "api/v1/config";
            var request = client.PrepareRequest(resource);

            var result = client.Execute(request);
            var config = DeserializeConfiguration(result);

            return config;
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
            var resource = "api/v1/config/version";
            var request = client.PrepareRequest(resource);

            var result = client.Execute(request);

            return result;
        }
    }
}

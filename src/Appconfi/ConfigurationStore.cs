using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Appconfi
{
    public class ConfigurationStore : IConfigurationStore
    {
        private const string ROUTE = "api/v1/features";
        private readonly AppconfiClient client;

        public ConfigurationStore(AppconfiClient client)
        {
            this.client = client;
        }

        public Dictionary<string, dynamic> GetFeatures()
        {
            var resource = ROUTE;
            var request = client.PrepareRequest(resource);

            var result = client.Execute(request);
            var config = Deserialize(result);

            return config;
        }

        public static Dictionary<string, dynamic> Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(json));

            var result = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            return result;
        }

        public string GetVersion()
        {
            var resource = $"{ROUTE}/version";
            var request = client.PrepareRequest(resource);

            var result = client.Execute(request);

            return result;
        }
    }
}

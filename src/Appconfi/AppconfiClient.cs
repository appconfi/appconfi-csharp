namespace Appconfi
{
    using RestSharp;

    public class AppconfiClient
    {
        private static string APIUri = "https://appconfi.com";

        public AppconfiClient(
            string applicationId, 
            string apiKey, 
            string environment = "[default]")
        {
            Api = new RestClient(APIUri);
            ApplicationId = applicationId;
            ApiKey = apiKey;
            Environment = environment;
        }

        public RestRequest PrepareRequest(string resource)
        {
            var request = new RestRequest(resource, Method.GET);

            request
               .AddParameter("key", ApiKey)
               .AddParameter("env", Environment)
               .AddParameter("app", ApplicationId);

            return request;
        }

        public RestClient Api { get; }

        public string ApplicationId { get; }

        public string ApiKey { get; }

        public string Environment { get; }
    }
}

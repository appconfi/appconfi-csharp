using System;
using System.Net.Http;

namespace Appconfi
{
    public class AppconfiClient
    {
        public static Uri AppconfiBaseURI = new Uri("https://appconfi.com");
        private Uri baseUri = AppconfiBaseURI;

        public AppconfiClient(
            Uri baseAddress,
            string applicationId,
            string apiKey,
            string environment = "[default]"):this(applicationId, apiKey,environment)
        {
            baseUri = baseAddress;
        }

        public AppconfiClient(
            string applicationId,
            string apiKey,
            string environment = "[default]")
        {
            ApplicationId = applicationId;
            ApiKey = apiKey;
            Environment = environment;
        }

        public string PrepareRequest(string resource)
        {

            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString["key"] = ApiKey;
            queryString["env"] = Environment;
            queryString["app"] = ApplicationId;

            return $"{resource}?{queryString.ToString()}";
        }

        public string Execute(string resource)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("X-Appconfi-UA", "AppConfi-Client .NET v1");
                client.BaseAddress = baseUri;

                var response = client.GetAsync(resource).Result;

                if (!response.IsSuccessStatusCode)
                    throw new BadRequestException(response);

                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public string ApplicationId { get; }

        public string ApiKey { get; }

        public string Environment { get; }
    }
}

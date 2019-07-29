using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Appconfi
{

    public class AppconfiClient
    {
        private static Uri APIUri = new Uri("https://appconfi.com");

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

        public async Task<string> ExecuteAsync(string resource)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("X-Appconfi-UA", "AppConfi-Client .NET v1");
                client.BaseAddress = APIUri;

                var response = await client.GetAsync(resource);

                if (!response.IsSuccessStatusCode)
                    throw new BadRequestException(response);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public string ApplicationId { get; }

        public string ApiKey { get; }

        public string Environment { get; }
    }
}

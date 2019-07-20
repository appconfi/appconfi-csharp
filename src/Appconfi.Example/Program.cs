namespace Appconfi.Example
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var applicationId = "application_id_here";
            var apiKey = "application_secret_key";
            var env = "[default]";

            var manager = Configuration.NewInstance(applicationId, apiKey, env, TimeSpan.FromSeconds(10));
            manager.StartMonitor();

            var task = Task.Factory.StartNew(() =>
            {
                CheckSetting(manager).Wait();

            }, TaskCreationOptions.LongRunning);

            Task.WaitAll(task);
        }

        public static async Task CheckSetting(AppconfiManager manager)
        {
            while (true)
            {
                var color = manager.GetSetting("application.color");
                var status = manager.IsFeatureEnabled("you.feature");

                Console.WriteLine($"is_enabled: {status.ToString().ToLower()}");
                Console.WriteLine($"color: {color}");

                await Task.Delay(2000);
            }
        }
    }
}

namespace Appconfi.Example
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var applicationId = "dc97d669-1460-4602-8ae3-2a35b2708df7";
            var apiKey = "a7822a44-af94-4f0c-9337-7c31f2fe33af";
            var env = "[default]";

            var manager = Configuration.NewInstance(
                applicationId, 
                apiKey, 
                env, 
                TimeSpan.FromMinutes(2), logger: new MyLogger());


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
                var user = new User("qwerty@example.com")
                              .AddProperty("profile", "premium")
                              .AddProperty("country", "Spain");


                var appVersion = manager.GetSetting("app_version");
                var isFeatureEnabled = manager.IsFeatureEnabled("my_awesome_feature");
                var isFeatureEnabledForUser = manager.IsFeatureEnabled("my_awesome_feature", user,false);

                Console.WriteLine($"{nameof(appVersion)}:{appVersion}");
                Console.WriteLine($"{nameof(isFeatureEnabled)}:{isFeatureEnabled}");
                Console.WriteLine($"{nameof(isFeatureEnabledForUser)}:{isFeatureEnabledForUser}");

                await Task.Delay(10000);
            }
        }

        public class MyLogger : ILogger
        {
            public void Error(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

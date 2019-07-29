namespace Appconfi.Example
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var applicationId = "<APP-ID>";
            var apiKey = "<API-KEY>";
            var env = "[default]";

            var manager = Configuration.NewInstance(
                applicationId, 
                apiKey, 
                env, 
                TimeSpan.FromSeconds(10),
                new ConsoleLogger());


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

        public class ConsoleLogger : ILogger
        {
            public void Error(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

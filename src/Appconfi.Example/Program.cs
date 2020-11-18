namespace Appconfi.Example
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var applicationId = "";
            var apiKey = "";
            var env = "";

            var manager = Configuration.NewInstance(
                new Uri(""),
                applicationId, 
                apiKey, 
                env, 
                TimeSpan.FromMinutes(1), logger: new MyLogger());


            manager.StartMonitor();

            var task = Task.Factory.StartNew(() =>
            {
                Watch(manager).Wait();

            }, TaskCreationOptions.LongRunning);

            Task.WaitAll(task);
        }

        public static async Task Watch(AppconfiManager manager)
        {
            while (true)
            {
                var status = manager.IsFeatureEnabled("new_feature", new User("123") { { "country", "MX" } });
                Console.WriteLine($"is_enabled: {status.ToString().ToLower()}");
                manager.ForceRefresh();
                await Task.Delay(5000);
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

namespace Appconfi.Example
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var applicationId = "530cfb60-da0a-491b-bad8-ff7122100bc1";
            var apiKey = "655759a7573e480f9d727ddf5ae31264";
            var env = "ES";

            var manager = Configuration.NewInstance(
                new Uri("https://localhost:5001"),
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
                var color = manager.GetSetting("application.color");
                var status = manager.IsFeatureEnabled("you.feature");

                Console.WriteLine($"is_enabled: {status.ToString().ToLower()}");
                Console.WriteLine($"color: {color}");
                manager.ForceRefresh();
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

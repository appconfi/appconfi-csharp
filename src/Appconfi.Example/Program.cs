namespace Appconfi.Example
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var applicationId = "03248339-139a-40e0-98db-46cc9d924362";
            var apiKey = "d7033c4f938b4895bb92f364175d12c8";
            var env = "prd";

            var manager = Configuration.NewInstance(
                new Uri("https://localhost:44389"),
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
                //var status = manager.IsFeatureEnabled("ale_feature");
                //var status = manager.IsFeatureEnabled("simple feature on");
                //var status = manager.IsFeatureEnabled("my_other_feature", new User("123") { { "country", "MX" } });
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

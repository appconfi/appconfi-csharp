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
                TimeSpan.FromSeconds(10));


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

                await Task.Delay(10000);
            }
        }
    }
}

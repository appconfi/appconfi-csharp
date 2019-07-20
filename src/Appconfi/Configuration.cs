namespace Appconfi
{
    using System;

    /// <summary>
    /// Application configuration.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Instance a configuration manager
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="apiKey">Secret application key</param>
        /// <param name="environmentName">Configuration environment</param>
        /// <param name="updateInterval">Interval for monitoring the configuration</param>
        /// <param name="settingsFallback">Settings fall-back in case of disconnection or internal settings</param>
        /// <param name="togglesFallback">Feature toggles fall-back in case of disconnection or internal configuration</param>
        public static AppconfiManager NewInstance(
            string applicationId, 
            string apiKey, 
            string environmentName, 
            TimeSpan updateInterval,
            Func<string,string> settingsFallback = null,
            Func<string,bool> togglesFallback = null)
        {
            if (updateInterval < TimeSpan.FromSeconds(10))
                throw new ArgumentOutOfRangeException("The update interval is less than 10 second");

            var client = new AppconfiClient(applicationId, apiKey, environmentName);
            var store = new ConfigurationStore(client);

            return new AppconfiManager(store, updateInterval, settingsFallback, togglesFallback);
        }

        /// <summary>
        /// Instance a configuration manager
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="apiKey">Secret application key</param>
        public static AppconfiManager NewInstance(
           string applicationId,
           string apiKey)
        {
            var client = new AppconfiClient(applicationId, apiKey);
            var store = new ConfigurationStore(client);

            return new AppconfiManager(store);
        }
    }
}

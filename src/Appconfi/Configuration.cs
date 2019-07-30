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
        /// <param name="logger">Application logger</param>
        /// <param name="updateInterval">Interval for monitoring the configuration</param>
        /// <param name="settingsFallback">Get setting from local configuration in case of disconnection</param>
        /// <param name="togglesFallback">Get feature toggle from local configuration in case of disconnection</param>
        public static AppconfiManager NewInstance(
            string applicationId,
            string apiKey,
            string environmentName,
            TimeSpan updateInterval,
            Func<string, string> getLocalSetting = null,
            Func<string, bool> getLocalFeature = null,
            ILogger logger = null
            )
        {
            if (updateInterval < TimeSpan.FromMinutes(1))
                throw new ArgumentOutOfRangeException("The update interval should be more than 1 minute");

            var client = new AppconfiClient(applicationId, apiKey, environmentName);
            var store = new ConfigurationStore(client);

            return new AppconfiManager(store, updateInterval, getLocalSetting, getLocalFeature, logger);
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

namespace Appconfi
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class AppconfiManager : IDisposable, IConfigurationManager
    {
        private const string FEATURE_ON = "on";
        private readonly IConfigurationStore store;
        readonly TimeSpan interval;
        private bool monitoring;

        readonly SemaphoreSlim timerSemaphore = new SemaphoreSlim(1);
        readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        readonly SemaphoreSlim syncCacheSemaphore = new SemaphoreSlim(1);

        IDictionary<string, string> settingsCache;
        IDictionary<string, string> togglesCache;

        private readonly ILogger logger;
        string currentVersion;
        DateTime lastTimeUpdated;

        public AppconfiManager(
           IConfigurationStore store,
           TimeSpan interval,
           ILogger logger
          ) 
        {
            this.logger = logger;
            this.store = store;
            this.interval = interval;
        }

        public AppconfiManager(
           IConfigurationStore store
           ) : this(store, TimeSpan.FromMinutes(1))
        {
        }

        public AppconfiManager(
            IConfigurationStore store,
            TimeSpan interval): this(store,interval,null)
        {
            
        }

        /// <summary>
        /// Check to see if the current instance is monitoring for changes
        /// </summary>
        public bool IsMonitoring => monitoring;

        /// <summary>
        /// Start the background monitoring for configuration changes in the central store
        /// </summary>
        public void StartMonitor()
        {
            if (IsMonitoring)
            {
                return;
            }

            try
            {
                timerSemaphore.Wait();

                //Check again to make sure we are not already running.
                if (this.IsMonitoring)
                {
                    return;
                }

                monitoring = true;
            }
            finally
            {
                timerSemaphore.Release();
            }
        }

        /// <summary>
        /// Stop Monitoring for Configuration Changes
        /// </summary>
        public void StopMonitor()
        {
            try
            {
                timerSemaphore.Wait();

                monitoring = false;
            }
            finally
            {
                timerSemaphore.Release();
            }
        }

        public void Dispose()
        {
            timerSemaphore.Dispose();
            cacheLock.Dispose();
            syncCacheSemaphore.Dispose();
        }

        /// <summary>
        /// Retrieve application setting from the local cache. Cloud configuration override local cache
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public string GetSetting(string setting, string defaultValue = null)
        {
            CheckForConfigurationChanges();

            if (string.IsNullOrEmpty(setting))
                throw new ArgumentNullException(nameof(setting), "Value cannot be null or empty.");

            string value;
            try
            {
                cacheLock.EnterReadLock();

                if (settingsCache == null || !settingsCache.TryGetValue(setting, out value))
                {
                    value = defaultValue;
                }
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return value;
        }

        /// <summary>
        /// Retrieve application feature toggle from the local cache.
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public bool IsFeatureEnabled(string feature, bool defaultValue= false)
        {
            CheckForConfigurationChanges();

            if (string.IsNullOrEmpty(feature))
                throw new ArgumentNullException(nameof(feature), "Value cannot be null or empty.");

            bool value = false;

            try
            {
                cacheLock.EnterReadLock();

                if (togglesCache != null && togglesCache.TryGetValue(feature, out string sv))
                    value = sv == FEATURE_ON;
                else
                    value = defaultValue;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return value;
        }

        public void ForceRefresh()
        {
           CheckForConfigurationChanges(forceRefresh: true);
        }

        private void CheckForConfigurationChanges(bool forceRefresh = false)
        {
            try
            {
                //Is not is monitoring changes exit
                if (!IsMonitoring && currentVersion != null)
                    return;

                //If the cache is still good 
                if (!forceRefresh && DateTime.UtcNow.Add(-interval) < lastTimeUpdated)
                    return;
                
                var latestVersion =  store.GetVersion();
                lastTimeUpdated = DateTime.UtcNow;

                // If the versions are the same, nothing has changed in the configuration.
                if (currentVersion == latestVersion) return;

                // Get the latest settings from the settings store and publish changes.
                var latestConfiguration = store.GetConfiguration();

                // Refresh the settings cache.
                try
                {
                    cacheLock.EnterWriteLock();

                    settingsCache = latestConfiguration?.Settings;
                    
                    togglesCache = latestConfiguration?.Toggles;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
                // Update the current version.
                currentVersion = latestVersion;

            }
            catch(Exception e)
            {
                if (logger != null)
                    logger.Error(e);
            }
        }

    }
}

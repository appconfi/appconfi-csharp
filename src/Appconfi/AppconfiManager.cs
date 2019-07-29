namespace Appconfi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class AppconfiManager : IDisposable, IConfigurationManager
    {
        private readonly IConfigurationStore store;

        readonly CancellationTokenSource cts = new CancellationTokenSource();
        Task monitoringTask;
        readonly TimeSpan interval;

        readonly SemaphoreSlim timerSemaphore = new SemaphoreSlim(1);
        readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        readonly SemaphoreSlim syncCacheSemaphore = new SemaphoreSlim(1);

        IDictionary<string, string> settingsCache;
        IDictionary<string, string> togglesCache;

        Func<string, string> getLocalSetting;
        Func<string, bool> getLocalToggle;
        private readonly ILogger logger;
        string currentVersion;
        public Action<KeyValuePair<string,string>> OnChange;

        public AppconfiManager(
           IConfigurationStore store,
           TimeSpan interval,
           Func<string, string> getLocalSetting,
           Func<string, bool> getLocalToggle,
           ILogger logger
          ) 
        {
            this.logger = logger;
            this.getLocalSetting = getLocalSetting;
            this.getLocalToggle = getLocalToggle;
            this.store = store;
            this.interval = interval;
        }

        public AppconfiManager(
            IConfigurationStore store,
            TimeSpan interval,
            Func<string, string> getLocalSetting,
            Func<string, bool> getLocalToggle
           ) : this(store, interval, getLocalSetting, getLocalToggle, null)
        {
        }

        public AppconfiManager(
           IConfigurationStore store
           ) : this(store, TimeSpan.FromMinutes(1))
        {
        }

        public AppconfiManager(
            IConfigurationStore store,
            TimeSpan interval): this(store,interval,null, null,null)
        {
            
        }

        /// <summary>
        /// Check to see if the current instance is monitoring for changes
        /// </summary>
        public bool IsMonitoring => this.monitoringTask != null && !this.monitoringTask.IsCompleted;

        /// <summary>
        /// Start the background monitoring for configuration changes in the central store
        /// </summary>
        public void StartMonitor()
        {
            if (this.IsMonitoring)
            {
                return;
            }

            try
            {
                this.timerSemaphore.Wait();

                //Check again to make sure we are not already running.
                if (this.IsMonitoring)
                {
                    return;
                }

                //Start running our task loop.
                this.monitoringTask = ConfigChangeMonitor();
            }
            finally
            {
                this.timerSemaphore.Release();
            }
        }

        /// <summary>
        /// Loop that monitors for configuration changes
        /// </summary>
        /// <returns></returns>
        public async Task ConfigChangeMonitor()
        {
            while (!cts.Token.IsCancellationRequested)
            {
                CheckForConfigurationChanges();
                await Task.Delay(interval, cts.Token);
            }
        }

        /// <summary>
        /// Stop Monitoring for Configuration Changes
        /// </summary>
        public void StopMonitor()
        {
            try
            {
                this.timerSemaphore.Wait();

                //Signal the task to stop
                this.cts.Cancel();

                //Wait for the loop to stop
                if (!monitoringTask.IsCanceled)
                    this.monitoringTask.Wait();

                this.monitoringTask = null;
            }
            finally
            {
                this.timerSemaphore.Release();
            }
        }

        public void Dispose()
        {
            this.cts.Cancel();
            timerSemaphore.Dispose();
            cacheLock.Dispose();
            syncCacheSemaphore.Dispose();
            cts.Dispose();
        }

        /// <summary>
        /// Retrieve application setting from the local cache. Cloud configuration override local cache
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public string GetSetting(string setting)
        {
            if (currentVersion == null)
                CheckForConfigurationChanges();

            if (string.IsNullOrEmpty(setting))
                throw new ArgumentNullException(nameof(setting), "Value cannot be null or empty.");

            string value;
            try
            {
                cacheLock.EnterReadLock();

                if (settingsCache == null || !settingsCache.TryGetValue(setting, out value))
                {
                    TryGetSettingValueFromLocal(setting, out value);
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
        public bool IsFeatureEnabled(string feature)
        {
            if (currentVersion == null)
                CheckForConfigurationChanges();

            if (string.IsNullOrEmpty(feature))
                throw new ArgumentNullException(nameof(feature), "Value cannot be null or empty.");

            bool value = false;

            try
            {
                cacheLock.EnterReadLock();

                if (togglesCache != null && togglesCache.TryGetValue(feature, out string sv))
                    value = sv == "on";
                else
                    TryGetToggleValueFromLocal(feature, out value);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }

            return value;
        }

        private bool TryGetSettingValueFromLocal(string key, out string value)
        {
            value = null;

            if (getLocalSetting == null)
                return false;

            value = getLocalSetting(key);
            return !string.IsNullOrEmpty(value);
        }

        private bool TryGetToggleValueFromLocal(string key, out bool value)
        {
            value = false;

            if (getLocalToggle == null)
                return false;

            value = getLocalToggle(key);

            return true;
        }

        private void KeyValueChange(KeyValuePair<string,string> keyValue)
        {
            if (OnChange == null)
                return;

            OnChange(keyValue);
        }

        public void ForceRefresh()
        {
           CheckForConfigurationChanges();
        }

        private void CheckForConfigurationChanges()
        {
            try
            {
                var latestVersion =  store.GetVersion();

                // If the versions are the same, nothing has changed in the configuration.
                if (currentVersion == latestVersion) return;

                // Get the latest settings from the settings store and publish changes.
                var latestConfiguration = store.GetConfiguration();

                // Refresh the settings cache.
                try
                {
                    cacheLock.EnterWriteLock();

                    if (settingsCache != null)
                    {
                        //Notify settings changed
                        latestConfiguration.Settings.Except(settingsCache).ToList().ForEach(kv => KeyValueChange(kv));
                    }
                    settingsCache = latestConfiguration?.Settings;

                    if (togglesCache != null)
                    {
                        //Notify settings changed
                        latestConfiguration.Toggles.Except(togglesCache).ToList().ForEach(kv => KeyValueChange(kv));
                    }
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

using System;
using System.Reflection;

namespace Appconfi.Web
{
    public class AppconfiOptions
    {
        public AppconfiCacheConfiguration FeatureToggleCacheConfiguration { get; private set; }
        public string BaseAddress { get; set; }
        public string Application { get; set; }
        public string Key { get; set; }
        public string Environment { get; set; } = "[default]";
        public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(5);

        public AppconfiOptions UseFeatureToggleCache(Assembly assembly, string resourceFileName)
        {
            FeatureToggleCacheConfiguration = new AppconfiCacheConfiguration
            {
                Assembly = assembly,
                EmbeddedFilePath = resourceFileName
            };
            return this;
        }
    }
}
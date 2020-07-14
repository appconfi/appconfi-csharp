using Microsoft.Extensions.DependencyInjection;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Appconfi.Web.Extensions.DependencyInjection
{
    public static class AppconfiServiceCollectionExtensions
    {
        private static Func<string, bool> GetLocalFeatures(AppconfiCacheConfiguration config)
        {
            if (config == null)
                return null;

            var embeddedResource = ResourceHelper.GetEmbeddedResource(config.EmbeddedFilePath, config.Assembly);
            IDictionary<string,string> cache = JsonConvert.DeserializeObject<Dictionary<string,string>>(embeddedResource);
            Func<string, bool> response = (feature) => {
                if (cache.ContainsKey(feature))
                    return cache[feature] == "on";
                return false;
            };

            return response;
        }

        public static IServiceCollection AddAppconfi(this IServiceCollection services, Action<AppconfiOptions> configure)
        {
            var options = new AppconfiOptions();
            configure(options);

            var manager = Configuration.NewInstance(
                new Uri(options.BaseAddress),
                options.Application,
                options.Key,
                options.Environment,
                options.CacheExpirationTime,
                null,
                GetLocalFeatures(options.FeatureToggleCacheConfiguration)
                );

            manager.StartMonitor();
            services.AddSingleton<IFeatureManager>(new FeatureManager(manager));

            return services;
        }
    }
}

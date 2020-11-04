using Microsoft.Extensions.DependencyInjection;
using System;

namespace Appconfi.Web.Extensions.DependencyInjection
{
    public static class AppconfiServiceCollectionExtensions
    {
        public static IServiceCollection AddAppconfi(this IServiceCollection services, Action<AppconfiOptions> configure)
        {
            var options = new AppconfiOptions();
            configure(options);

            var manager = Configuration.NewInstance(
                new Uri(options.BaseAddress),
                options.Application,
                options.Key,
                options.Environment,
                options.CacheExpirationTime
                );

            manager.StartMonitor();
            services.AddSingleton<IAppconfiFeatureManager>(new FeatureManager(manager));

            return services;
        }
    }
}

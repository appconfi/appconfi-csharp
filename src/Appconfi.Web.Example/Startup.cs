using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Appconfi.Web.Extensions.DependencyInjection;

namespace Appconfi.Web.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAppconfi(options => {
                options.Application = "530cfb60-da0a-491b-bad8-ff7122100bc1";
                options.Key = "655759a7573e480f9d727ddf5ae31264";
                options.Environment = "ES";
                options.BaseAddress = "https://localhost:5001";
                options.CacheExpirationTime = TimeSpan.FromMinutes(1);
                options.UseFeatureToggleCache(typeof(Startup).Assembly, "features.json");               
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

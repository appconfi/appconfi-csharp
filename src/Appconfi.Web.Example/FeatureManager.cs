using System;

namespace Appconfi.Web.Example
{
    public class FeatureManager : IFeatureManager
    {
        Lazy<AppconfiManager> instance = new Lazy<AppconfiManager>(() =>
       {
           var applicationId = "dc97d669-1460-4602-8ae3-2a35b2708df7";
           var apiKey = "a7822a44-af94-4f0c-9337-7c31f2fe33af";
           var env = "[default]";

           var manager = Configuration.NewInstance(
               applicationId,
               apiKey,
               env,
               TimeSpan.FromMinutes(1));

           manager.StartMonitor();

           return manager;
       });
        
        public bool IsEnable(string feature)
        {
            return instance.Value.IsFeatureEnabled(feature);
        }
    }
}
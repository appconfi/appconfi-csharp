using System.Collections.Generic;

namespace Appconfi
{
    public class LocalAppconfiManager : AppconfiManagerBase, IConfigurationManager
    {
        private IDictionary<string, dynamic> featuresCache;

        public LocalAppconfiManager(string json)
        {
            featuresCache = ConfigurationStore.Deserialize(json);
        }

        public bool IsFeatureEnabled(string feature, bool defaultValue = false)
        {
            var value = defaultValue;
            try
            {
                if (featuresCache != null && featuresCache.TryGetValue(feature, out object status))
                    value = IsEnabled(status, defaultValue);
            }
            catch { }
            return value;
        }

        public bool IsFeatureEnabled(string feature, User user, bool defaultValue = false)
        {
            var value = defaultValue;
            try
            {
                if (featuresCache != null && featuresCache.TryGetValue(feature, out object status))
                    value = IsEnabled(status, user, defaultValue);
            }
            catch { }
            return value;
        }
    }
}
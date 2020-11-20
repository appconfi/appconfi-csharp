namespace Appconfi
{
    public abstract class AppconfiManagerBase
    {
        protected bool IsEnabled(object status, bool defaultValue)
        {
            if (status is bool isEnabled)
            {
                return isEnabled;
            }
            var property = ((dynamic)status)["isEnabled"];
            if (property?.Value != null && property.Value.GetType() == typeof(bool))
            {
                return (bool)property;
            }

            return defaultValue;
        }
        protected bool IsEnabled(object status, User user, bool defaultValue)
        {
            if (status is bool)
            {
                return defaultValue;
            }
            return user.IsFeatureEnabled((status as dynamic).enabledFor, defaultValue);
        }
    }
}

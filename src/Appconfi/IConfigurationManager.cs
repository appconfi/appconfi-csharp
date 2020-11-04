namespace Appconfi
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// Get the settings for a given environment
        /// </summary>
        /// <param name="key">Setting name</param>
        /// <returns>Value for the given setting</returns>
        string GetSetting(string key, string defaultValue = null);

        /// <summary>
        /// Determines whether a feature test is enabled for a given environment
        /// </summary>
        /// <param name="key">Feature name</param>
        /// <returns>If the feature toggle is enabled</returns>
        bool IsFeatureEnabled(string key, bool defaultValue = false);
    }
}
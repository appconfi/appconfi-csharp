namespace Appconfi
{
    public interface IConfigurationManager
    {
        /// <summary>
        /// Determines whether a feature is enabled
        /// </summary>
        /// <param name="feature">Feature name</param>
        /// <returns>If the feature is enabled</returns>
        bool IsFeatureEnabled(string feature, bool defaultValue = false);

        /// <summary>
        /// Determines whether a feature is enabled for a given user
        /// </summary>
        /// <param name="feature">Feature name</param>
        /// <param name="user">User</param>
        /// <returns>If the feature is enabled</returns>
        bool IsFeatureEnabled(string feature, User user, bool defaultValue = false);
    }
}
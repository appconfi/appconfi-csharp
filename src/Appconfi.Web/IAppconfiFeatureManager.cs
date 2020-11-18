namespace Appconfi.Web
{
    public interface IAppconfiFeatureManager
    {
        void ForceRefresh();
        bool IsEnabled(string featureName, bool defaultValue = false);
        bool IsEnabled(string featureName, User user, bool defaultValue = false);
    }
}
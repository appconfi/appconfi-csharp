namespace Appconfi.Web
{
    public interface IAppconfiFeatureManager
    {
        void ForceRefresh();
        bool IsEnabled(string featureName, bool defaultValue = false);
    }
}
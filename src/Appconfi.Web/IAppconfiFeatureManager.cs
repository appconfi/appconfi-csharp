namespace Appconfi.Web
{
    public interface IAppconfiFeatureManager
    {
        void ForceRefresh();
        bool IsEnabled(string feature, bool defaultValue = false);
        bool IsEnabled(string feature, User user, bool defaultValue = false);
    }
}
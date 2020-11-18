namespace Appconfi.Web
{
    public class FeatureManager : IAppconfiFeatureManager
    {
        private readonly AppconfiManager manager;

        public FeatureManager(AppconfiManager manager)
        {
            this.manager = manager;
        }
        public bool IsEnabled(string featureName, bool defaultValue = false)
        {
            return manager.IsFeatureEnabled(featureName, defaultValue);
        }

        public void ForceRefresh()
        {
            manager.ForceRefresh();
        }

        public bool IsEnabled(string featureName, User user, bool defaultValue = false)
        {
            return manager.IsFeatureEnabled(featureName, user, defaultValue);
        }
    }
}

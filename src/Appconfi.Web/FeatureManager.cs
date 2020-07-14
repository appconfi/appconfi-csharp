namespace Appconfi.Web
{
    public class FeatureManager : IFeatureManager
    {
        private readonly AppconfiManager manager;

        public FeatureManager(AppconfiManager manager)
        {
            this.manager = manager;
        }
        public bool IsEnabled(string featureName)
        {
            return manager.IsFeatureEnabled(featureName);
        }
    }
}

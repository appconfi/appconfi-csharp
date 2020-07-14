namespace Appconfi.Web
{
    public interface IFeatureManager
    {
        bool IsEnabled(string featureName);
    }
}
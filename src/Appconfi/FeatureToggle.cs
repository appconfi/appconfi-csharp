namespace Appconfi
{
    public static class FeatureToggle
    {
        public static string On => "on";

        public static string Off => "off";

        public static bool IsEnabled(string value)
        {
            return value == On;
        }
    }
}

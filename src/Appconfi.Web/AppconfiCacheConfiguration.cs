namespace Appconfi.Web
{
    using System.Reflection;
    public class AppconfiCacheConfiguration
    {
        public Assembly Assembly { get; set; }
        public string EmbeddedFilePath { get; set; }
    }
}
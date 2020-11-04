using System;

namespace Appconfi.Web
{
    public class AppconfiOptions
    {
        public string BaseAddress { get; set; }
        public string Application { get; set; }
        public string Key { get; set; }
        public string Environment { get; set; } = "[default]";
        public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(5);
    }
}
namespace Appconfi
{
    using System.Collections.Generic;

    public class ApplicationConfiguration
    {
        public IDictionary<string,string> Settings { get; set; }

        public IDictionary<string,string> Toggles { get; set; }
    }
}

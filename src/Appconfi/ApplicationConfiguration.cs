namespace Appconfi
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ApplicationConfiguration
    {
        [DataMember(Name = "settings")]
        public IDictionary<string,string> Settings { get; set; }

        [DataMember(Name = "toggles")]

        public IDictionary<string,string> Toggles { get; set; }
    }

}

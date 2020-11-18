namespace Appconfi
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ApplicationConfiguration : Dictionary<string, object>
    {
    }

}

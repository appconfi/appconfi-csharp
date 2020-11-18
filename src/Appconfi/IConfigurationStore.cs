using System.Collections.Generic;

namespace Appconfi
{
    public interface IConfigurationStore
    {
        string GetVersion();

        Dictionary<string, dynamic> GetFeatures();
    }
}
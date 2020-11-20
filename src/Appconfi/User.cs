using System.Collections.Generic;

namespace Appconfi
{
    public class User : Dictionary<string, string>
    {
        public User(string id)
        {
            Add("identifier", id);
            Add("id", id);
        }
        public User(IDictionary<string, string> properties)
        {
            foreach (var p in properties)
            {
                Add(p.Key, p.Value);
            }
        }

        internal bool IsFeatureEnabled(dynamic rule, bool defaultValue)
        {
            if (rule == null)
                return defaultValue;
            try
            {
                if (rule.name == "property")
                {
                    return new ByPropertyRule
                    {
                        Option = rule.parameters.option,
                        Property = rule.parameters.property,
                        Values = rule.parameters.values
                    }.IsTarget(this);
                }
                else if (rule.name == "percentage")
                {
                    return new RulePercentage
                    {
                        Percent = rule.parameters.percent
                    }.IsTarget(this);
                }
            }
            catch
            {
                return defaultValue;
            }
            return defaultValue;
        }
    }
}
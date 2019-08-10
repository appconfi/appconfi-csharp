using System;
using System.Collections.Generic;
using System.Linq;

namespace Appconfi
{
    public class User
    {
        public IDictionary<string, string> _properties;
        public User(string id)
        {
            Id = id;
            _properties = new Dictionary<string, string>();
        }

        public string Id { get; }

        public User AddProperty(string name, string value)
        {
            ValidateArg(name);
            ValidateArg(value);

            _properties.Add(name, value);
            return this;
        }

        private void ValidateArg(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                throw new ArgumentNullException("Invalid property");

            var invalidChars = new[] { ",", ":" };
            if (invalidChars.Any(x => arg.Contains(x)))
                throw new ArgumentException("Invalid property/value. Can not contains : or ,");
        }

        public override string ToString()
        {
            //TODO: Encode URL
            return _properties.Aggregate($"id:{Id}", (a, b) => $"{a},{b.Key}:{b.Value}");
        }
    }
}
using System;

namespace Azarashi.Utilities.HTML
{
    public class Attribute : ICloneable
    {
        public string Name { get; }
        public string Value { get; }

        public Attribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public object Clone()
        {
            return new Attribute(Name, Value);
        }
    }
}
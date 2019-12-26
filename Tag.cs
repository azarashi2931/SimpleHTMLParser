using System.Collections.Generic;

namespace Azarashi.Utilities.HTML
{
    public class Tag : Dictionary<string, Attribute> 
    {
        public string Name { get; }

        public Tag(string name)
        {
            Name = name;
        }
    }
}
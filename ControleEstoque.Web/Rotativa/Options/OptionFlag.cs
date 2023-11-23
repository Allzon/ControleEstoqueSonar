using System;

namespace Rotativa.Options
{
    class OptionFlagAttribute : Attribute
    {
        public string Name { get; private set; }

        public OptionFlagAttribute(string name)
        {
            Name = name;
        }
    }
}

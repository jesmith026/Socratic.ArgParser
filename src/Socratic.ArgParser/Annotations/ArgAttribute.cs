using System;

namespace Socratic.ArgParser.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgAttribute : Attribute
    {
        public ArgAttribute(int index)
        {
            Index = index;
        }

        public int Index { get; set; }
        public bool Optional { get; set; } = false;
    }
}
using System;

namespace Socratic.ArgParser.Exceptions
{
    public class ArgumentCountException : Exception
    {
        public ArgumentCountException(Type targetType)
            : base($"Number of arguments provided don't match the number or serializable properties on the target class {targetType}")
        { }
    }
}
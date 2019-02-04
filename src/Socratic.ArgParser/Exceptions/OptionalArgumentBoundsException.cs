using System;

namespace Socratic.ArgParser.Exceptions
{
    public class OptionalArgumentBoundsException : Exception
    {
        public OptionalArgumentBoundsException(int optionalIndex, int requiredIndex)
            : base ($"Optional Index {optionalIndex} occurs before required index {requiredIndex}. All required indices must appear before any optional indices")
        { }
    }
}
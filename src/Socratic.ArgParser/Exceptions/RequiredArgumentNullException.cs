using System;

namespace Socratic.ArgParser.Exceptions
{
    public class RequiredArgumentNullException : Exception
    {
        public RequiredArgumentNullException(int index)
            : base($"Required parameter at index {index} cannot be null")
        {}
    }
}
using System;

namespace Socratic.ArgParser.Exceptions
{
    public class IndexNotFoundException : Exception
    {
        public IndexNotFoundException(int index)
            : base ($"No definition found for index {index}")
        { }
    }
}
using System;
using System.Reflection;
using Socratic.ArgParser.Annotations;

namespace Socratic.ArgParser.Exceptions
{
    public class DuplicateIndexException : Exception
    {
        public DuplicateIndexException(MemberInfo info)
            : base($"Duplicate index definition detected for property {info.Name} with index {info.GetCustomAttribute<ArgAttribute>().Index}")
        {}
    }
}
using Socratic.ArgParser.Annotations;
using Socratic.ArgParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Socratic.ArgParser
{
    public static class ArgParser
    {
        public static T Serialize<T>(string[] args)
        {
            if (args == null || args.Length == 0) return default(T);

            var result = Activator.CreateInstance<T>();     
            var (dict, requiredCount, _) = GetPropertiesSummary(typeof(T));  
             
            if (args.Length < requiredCount || args.Length > dict.Count)
                throw new ArgumentCountException(typeof(T));

            for (var index = 0; index < args.Length; index++)
            {
                if (string.Compare("null", args[index], StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if (index <= requiredCount-1)
                        throw new RequiredArgumentNullException(index);

                    continue;
                }

                if (!dict.TryGetValue(index, out var prop))
                    throw new IndexNotFoundException(index);

                var value = Convert.ChangeType(args[index], prop.PropertyType);

                prop.SetValue(result, value, null);
            }

            return result;
        }
        
        private static (Dictionary<int, PropertyInfo> PropertyDictionary, int RequiredIndexCount, int OptionalIndexCount) GetPropertiesSummary(Type type)
        {
            var dict = new Dictionary<int, PropertyInfo>();

            var optionalIndexList = new List<int>();
            var requiredIndexList = new List<int>();

            foreach (var prop in type.GetProperties())
            {
                var attribute = prop.GetCustomAttributes()
                    .OfType<ArgAttribute>()
                    .FirstOrDefault();

                if (attribute == null) continue;

                var index = attribute.Index;
                if (dict.ContainsKey(index))                
                    throw new DuplicateIndexException(prop);
                else                
                    dict.Add(index, prop);

                var optional = attribute.Optional;
                if (optional)
                    optionalIndexList.Add(index);
                else 
                    requiredIndexList.Add(index);
            }

            var maxRequiredIndex = requiredIndexList.Any() ? requiredIndexList.Max() : -1;
            var minOptionalIndex = optionalIndexList.Any() ? optionalIndexList.Min() : -1;
            if (maxRequiredIndex > -1 && minOptionalIndex > -1 && maxRequiredIndex > minOptionalIndex)
                throw new OptionalArgumentBoundsException(minOptionalIndex, maxRequiredIndex);

            return (dict, requiredIndexList.Count, optionalIndexList.Count);
        }
    }
}
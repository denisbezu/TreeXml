using System.Collections.Generic;
using System.Linq;

namespace TreeXml
{
    public static class ExtensionClass
    {
        public static Parameter GetByName(this IList<Parameter> parameters, string name)
        {
            return parameters.FirstOrDefault(p => p.Name.ToLower().Equals(name.ToLower()));
        }

        public static Parameter CheckStartsWith(this List<Parameter> parameters, string start)
        {
            return parameters.FirstOrDefault(p => p.Name.StartsWith(start));
        }

    }
}
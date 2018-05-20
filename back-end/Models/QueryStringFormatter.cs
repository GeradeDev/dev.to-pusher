using System.Collections.Generic;

namespace AwsDotnetCsharp
{
    public static class QueryStringFormatter
    {
        public static string Get(IDictionary<string, string> qsParms, string parameterName)
        {
            var result = string.Empty;
            if (qsParms == null) return result;
            if (qsParms.ContainsKey(parameterName))
            {
                result = qsParms[parameterName];
            }
            return result;
        }
    }
}
using System.Text.RegularExpressions;
using Dapper.FluentMap.Conventions;

namespace Dapper.FluentMap.TestConsole.Data.Mapping
{
    /// <summary>
    /// Represents a convention which transforms property names to database column names.
    /// </summary>
    public class PropertyTransformConvention : Convention
    {
        public PropertyTransformConvention()
        {
            // Map 'UrlOptimizedName' to 'Url_Optimized_Name'.
            Properties()
                .Configure(c => c.Transform(s => Regex.Replace(input: s, pattern: "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", replacement: "$1$3_$2$4")));
        }
    }
}

using Dapper.FluentMap.Conventions;

namespace App.Data.Mapping
{
    /// <summary>
    /// Represents a convention which adds type prefixes to database column names.
    /// </summary>
    public class TypePrefixConvention : Convention
    {
        public TypePrefixConvention()
        {
            // Map all properties of type int and with the name 'id' to column 'autID'.
            Properties<int>()
                .Where(c => c.Name.ToLower() == "id")
                .Configure(c => c.HasColumnName("autID"));

            // Prefix all properties of type string with 'str' when mapping to column names.
            Properties<string>()
                .Configure(c => c.HasPrefix("str"));

            // Prefix all properties of type string with 'str' when mapping to column names.
            Properties<string>()
                .Configure(c => c.HasPrefix("str"));

            // Prefix all properties of type int with 'int' when mapping to column names.
            Properties<int>()
                .Configure(c => c.HasPrefix("int"));
        }
    }
}

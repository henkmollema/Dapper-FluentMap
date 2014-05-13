namespace Dapper.FluentMap.Conventions
{
    public class ConventionPropertyConfiguration
    {
        internal string ColumnName { get; private set; }

        internal string Prefix { get; private set; }

        public ConventionPropertyConfiguration HasColumnName(string columnName)
        {
            ColumnName = columnName;
            return this;
        }

        public ConventionPropertyConfiguration HasPrefix(string prefix)
        {
            Prefix = prefix;
            return this;
        }
    }
}

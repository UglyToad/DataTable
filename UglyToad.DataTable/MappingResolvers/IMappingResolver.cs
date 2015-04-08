namespace UglyToad.DataTable.MappingResolvers
{
    using System.Data;
    using Types;

    public interface IMappingResolver
    {
        ExtendedPropertyInfo[] GetPropertyMappings<T>(DataTable dataTable, 
            DataTableParserSettings settings);
    }
}

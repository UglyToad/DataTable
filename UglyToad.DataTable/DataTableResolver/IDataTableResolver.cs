namespace UglyToad.DataTable.DataTableResolver
{
    using System.Collections.Generic;
    using System.Data;
    using DataTypeConverter;
    using Types;

    public interface IDataTableResolver
    {
        IList<T> ToObjects<T>(DataRow[] dataRows, IDataTypeConverter dataTypeConverter, ExtendedPropertyInfo[] mappings, DataTableParserSettings settings);
    }
}

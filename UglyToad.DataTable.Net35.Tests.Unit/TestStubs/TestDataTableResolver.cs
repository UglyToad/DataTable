namespace UglyToad.DataTable.Net35.Tests.Unit.TestStubs
{
    using System.Collections.Generic;
    using System.Data;
    using DataTableResolver;
    using DataTypeConverter;
    using Types;

    internal class TestDataTableResolver : IDataTableResolver
    {
        public IList<T> ToObjects<T>(DataRow[] dataRows, IDataTypeConverter dataTypeConverter, ExtendedPropertyInfo[] mappings, DataTableParserSettings settings)
        {
            return new List<T>();
        }
    }
}

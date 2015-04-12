namespace UglyToad.DataTable.Net35.Tests.Unit.TestStubs
{
    using System;
    using DataTypeConverter;
    using Types;

    internal class TestConverter : IDataTypeConverter
    {
        public object FieldToObject(object field, Type type, DataTableParserSettings settings, DbNullConverter dbNullConverter)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}

namespace UglyToad.DataTable.DataTypeConverter
{
    using System;
    using Types;

    public interface IDataTypeConverter
    {
        object FieldToObject(object field, Type type, DataTableParserSettings settings, DbNullConverter dbNullConverter);
    }
}

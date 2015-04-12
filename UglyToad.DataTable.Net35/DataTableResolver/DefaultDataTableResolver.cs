namespace UglyToad.DataTable.DataTableResolver
{
    using System.Collections.Generic;
    using System.Data;
    using DataTypeConverter;
    using Factories;
    using Types;

    internal class DefaultDataTableResolver : IDataTableResolver
    {
        public virtual IList<T> ToObjects<T>(DataRow[] dataRows, 
            IDataTypeConverter dataTypeConverter, 
            ExtendedPropertyInfo[] mappings, 
            DataTableParserSettings settings)
        {
            Guard.ArgumentNotNull(dataRows);
            Guard.ArgumentNotNull(dataTypeConverter);
            Guard.ArgumentNotNull(mappings);
            Guard.ArgumentNotNull(settings);

            var dbNullConverter = GetDbNullConverter(settings);

            var objectList = new T[dataRows.Length];

            for (int rowIndex = 0; rowIndex < dataRows.Length; rowIndex++)
            {
                var returnObject = ObjectInstantiator<T>.CreateNew();

                foreach (var mapping in mappings)
                {
                    object value = dataTypeConverter.FieldToObject(dataRows[rowIndex][mapping.ColumnIndex], 
                        mapping.PropertyInfo.PropertyType, 
                        settings, 
                        dbNullConverter);

                    mapping.PropertyInfo.SetValue(returnObject, value, null);
                }

                objectList[rowIndex] = returnObject;
            }

            return objectList;
        }

        protected virtual DbNullConverter GetDbNullConverter(DataTableParserSettings settings)
        {
            return new DbNullConverter(settings);
        }
    }
}

namespace UglyToad.DataTable.DataTableResolver
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using DataTypeConverter;
    using Factories;
    using Types;

    public class ParallelDataTableResolver : IDataTableResolver
    {
        public IList<T> ToObjects<T>(DataRow[] dataRows, IDataTypeConverter dataTypeConverter, ExtendedPropertyInfo[] mappings, DataTableParserSettings settings)
        {
            Guard.ArgumentNotNull(dataRows);
            Guard.ArgumentNotNull(dataTypeConverter);
            Guard.ArgumentNotNull(mappings);
            Guard.ArgumentNotNull(settings);

            ConcurrentBag<T> objectList = new ConcurrentBag<T>();
            var dbNullConverter = new DbNullConverter(settings);

            Parallel.For(0, dataRows.Length, (rowIndex) =>
            {
                T returnObject = ObjectInstantiator<T>.CreateNew();

                foreach (var mapping in mappings)
                {
                    object value = dataTypeConverter.FieldToObject(dataRows[rowIndex][mapping.ColumnIndex], mapping.PropertyInfo.PropertyType, settings, dbNullConverter);
                    mapping.PropertyInfo.SetValue(returnObject, value);
                }

                objectList.Add(returnObject);
            });

            return objectList.ToList();
        }
    }
}

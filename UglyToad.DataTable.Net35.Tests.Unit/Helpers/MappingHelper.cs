namespace UglyToad.DataTable.Net35.Tests.Unit.Helpers
{
    using System.Data;
    using System.Reflection;
    using Types;

    internal class MappingHelper
    {
        public static ExtendedPropertyInfo[] CreatePropertyMappingsDirectlyMatchingObject<T>()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            var extendedProperties = new ExtendedPropertyInfo[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                extendedProperties[i] = new ExtendedPropertyInfo(properties[i].Name, properties[i], -1);
            }

            return extendedProperties;
        }

        public static ExtendedPropertyInfo[] CreatePropertyMappingsMatchingTable<T>(DataTable dataTable)
        {
            var propertyInfos = CreatePropertyMappingsDirectlyMatchingObject<T>();

            foreach (var extendedPropertyInfo in propertyInfos)
            {
                extendedPropertyInfo.ColumnIndex = dataTable.Columns.IndexOf(extendedPropertyInfo.FieldName);
            }

            return propertyInfos;
        }
    }
}

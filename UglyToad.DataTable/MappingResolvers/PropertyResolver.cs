namespace UglyToad.DataTable.MappingResolvers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Types;

    internal class PropertyResolver
    {
        private const string Id = "id";

        public virtual void GenerateMappingsFromProperties(IList<ExtendedPropertyInfo> mappedProperties, 
            DataTable dataTable, 
            DataTableParserSettings settings, 
            PropertyInfo[] properties)
        {
            bool isFirstMapper = mappedProperties.Count == 0;

            foreach (PropertyInfo property in properties)
            {
                // If we must avoid overwrites we do so here.
                if (!isFirstMapper && !settings.SubsequentMappingsShouldOverwrite)
                {
                    if (mappedProperties.Count(p => p.PropertyInfo.Name == property.Name) > 0)
                    {
                        continue;
                    }
                }

                bool mappingFound = false;

                if (dataTable.Columns.Contains(property.Name))
                {
                    mappedProperties.Add(new ExtendedPropertyInfo(fieldName: property.Name, propertyInfo: property, columnIndex: dataTable.Columns.IndexOf(property.Name)));
                    mappingFound = true;
                }

                // Special case handling for Id columns/properties.
                if (!mappingFound && property.Name.ToLowerInvariant().Contains(Id))
                {
                    GenerateIdSpecificPropertyMappings(mappedProperties, dataTable, property);
                }
            }
        }

        private void GenerateIdSpecificPropertyMappings(IList<ExtendedPropertyInfo> mappedProperties, DataTable dataTable, PropertyInfo property)
        {
            string searchTerm = (string.Compare(property.Name, Id, true, CultureInfo.InvariantCulture) == 0) ? property.DeclaringType.Name + Id : Id;

            if (dataTable.Columns.Contains(searchTerm))
            {
                mappedProperties.Add(new ExtendedPropertyInfo(fieldName: searchTerm, propertyInfo: property, columnIndex: dataTable.Columns.IndexOf(searchTerm)));
            }
        }
    }
}

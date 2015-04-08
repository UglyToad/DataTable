namespace UglyToad.DataTable.MappingResolvers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using Enums;
    using Exceptions;
    using Types;

    /// <summary>
    /// Resolves mappings from an object's properties to DataTable columns.
    /// </summary>
    internal class DefaultMappingResolver : IMappingResolver
    {
        private static readonly AttributeResolver AttributeResolver = new AttributeResolver();
        private static readonly PropertyResolver PropertyResolver = new PropertyResolver();

        /// <summary>
        /// Gets property mappings for a specified type from a DataTable.
        /// </summary>
        /// <typeparam name="T">The type of object to map.</typeparam>
        /// <param name="dataTable">The DataTable to map from.</param>
        /// <param name="settings">The settings to use while mapping.</param>
        /// <returns>A list of the mappings.</returns>
        public ExtendedPropertyInfo[] GetPropertyMappings<T>(DataTable dataTable, DataTableParserSettings settings)
        {
            Guard.ArgumentNotNull(dataTable);
            Guard.ArgumentNotNull(settings);

            var mappedProperties = new List<ExtendedPropertyInfo>();

            var typeProperties = GetPropertiesForType<T>(settings.InheritMappings);

            // For non-overwriting mappings the execution order is important.
            switch (settings.MappingMatchOrder)
            {
                case MappingMatchOrder.PropertyNameFirst:
                    PropertyResolver.GenerateMappingsFromProperties(mappedProperties, dataTable, settings, typeProperties);
                    AttributeResolver.GenerateMappingsFromAttributes(mappedProperties, dataTable, settings, typeProperties);
                    break;

                case MappingMatchOrder.AttributeValueFirst:
                    AttributeResolver.GenerateMappingsFromAttributes(mappedProperties, dataTable, settings, typeProperties);
                    PropertyResolver.GenerateMappingsFromProperties(mappedProperties, dataTable, settings, typeProperties);
                    break;

                case MappingMatchOrder.IgnorePropertyNames:
                    AttributeResolver.GenerateMappingsFromAttributes(mappedProperties, dataTable, settings, typeProperties);
                    break;

                case MappingMatchOrder.IgnoreAttributes:
                    PropertyResolver.GenerateMappingsFromProperties(mappedProperties, dataTable, settings, typeProperties);
                    break;
            }

            CheckForErrors<T>(settings, mappedProperties, typeProperties);

            return mappedProperties.ToArray();
        }

        private static void CheckForErrors<T>(DataTableParserSettings settings, 
            List<ExtendedPropertyInfo> mappedProperties,
            PropertyInfo[] typeProperties)
        {
            // Error in mapping handling, checks for missing mappings.
            if (settings.MissingMappingHandling == MissingMappingHandling.Error
                && mappedProperties.Count < typeProperties.Length)
            {
                throw new MissingMappingException<T>();
            }

            // Checks for duplicate mappings in the list.
            if (!settings.AllowDuplicateMappings
                && mappedProperties.Select(p => p.ColumnIndex).Distinct().Count() != mappedProperties.Count)
            {
                throw new DuplicateMappingException<T>();
            }
        }

        protected virtual void VerifyMappingIndexIntegrity<T>(DataTable dataTable,
            IList<ExtendedPropertyInfo> mappings)
        {
            var columns = dataTable.Columns;

            int columnsCount = columns.Count;

            foreach (var mapping in mappings)
            {
                if (mapping == null)
                {
                    throw new InvalidMappingException<T>();
                }

                if (mapping.ColumnIndex < 0 || mapping.ColumnIndex >= columnsCount)
                {
                    if (columns.Contains(mapping.FieldName))
                    {
                        mapping.ColumnIndex = columns.IndexOf(mapping.FieldName);
                    }
                    else
                    {
                        throw new InvalidMappingException<T>();
                    }
                }
            }
        }

        private static PropertyInfo[] GetPropertiesForType<T>(bool inheritMappings)
        {
            // If we shouldn't inherit properties we need to declare the binding flags to ignore inherited properties.
            // All 3 flags are required for correct return.
            return inheritMappings ? typeof(T).GetProperties()
                : typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        }
    }
}

namespace UglyToad.DataTable
{
    using System.Collections.Generic;
    using System.Data;
    using DataTableResolver;
    using DataTypeConverter;
    using Enums;
    using MappingResolvers;
    using Types;

    /// <summary>
    /// Class responsible for converting <see cref="DataTable"/> to list of 
    /// specified type with default or custom conversion settings./>
    /// </summary>
    public class DataTableConverter
    {
        private DataTableParserSettings dataTableParserSettings = new DataTableParserSettings();
        private IMappingResolver mappingResolver = new DefaultMappingResolver();
        private IDataTypeConverter dataTypeConverter = new DefaultDataTypeConverter();

        private IDataTableResolver customResolver;

        public void UseCustomResolver(IDataTableResolver customResolver)
        {
            this.customResolver = customResolver;
        }

        /// <summary>
        /// Gets or sets the settings for parsing a DataTable.
        /// </summary>
        public virtual DataTableParserSettings DataTableParserSettings
        {
            get { return dataTableParserSettings; }
            set { dataTableParserSettings = value; }
        }

        public virtual IDataTypeConverter DataTypeConverter
        {
            get { return dataTypeConverter; }
            set { dataTypeConverter = value; }
        }

        /// <summary>
        /// Gets or sets the resolver used to map columns to properties.
        /// </summary>
        public virtual IMappingResolver MappingResolver
        {
            get { return mappingResolver; }
            set
            {
                mappingResolver = value;
            }
        }

        public static DataTableConverter Create()
        {
            return new DataTableConverter();
        }

        public static DataTableConverter Create(DataTableParserSettings settings)
        {
            var parser = new DataTableConverter {DataTableParserSettings = settings};

            return parser;
        }

        public static IList<T> Convert<T>(DataTable table)
        {
            return Convert<T>(table, new DataTableParserSettings());
        }

        public static IList<T> Convert<T>(DataTable table, DataTableParserSettings settings)
        {
            return new DataTableConverter().ConvertToObjectList<T>(table, settings);
        }

        /// <summary>
        /// Converts DataTable to object enumerable.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="table">The <see cref="DataTable"/> to convert.</param>
        /// <returns>An IEnumerable&lt;T&gt; with objects initialized.</returns>
        public virtual IList<T> ConvertToObjectList<T>(DataTable table)
        {
            return ToObjectsInternal<T>(table, dataTableParserSettings);
        }

        public virtual IList<T> ConvertToObjectList<T>(DataTable table, DataTableParserSettings dataTableParserSettings)
        {
            return ToObjectsInternal<T>(table, dataTableParserSettings);
        }

        protected virtual IList<T> ToObjectsInternal<T>(DataTable table, DataTableParserSettings dataTableParserSettings)
        {
            return GetConverter(dataTableParserSettings).ConvertToType<T>(table);
        }

        private ConversionManager GetConverter(DataTableParserSettings dataTableParserSettings)
        {
            return new ConversionManager(dataTableParserSettings, 
                mappingResolver, 
                GetResolver(), 
                dataTypeConverter);
        }

        private IDataTableResolver GetResolver()
        {
            if (customResolver != null)
            {
                return customResolver;
            }

            switch (dataTableParserSettings.Resolver)
            {
                case Resolver.Delegate:
                    return new DelegateDataTableResolver();
                default:
                    return new DefaultDataTableResolver();
            }
        }
    }
}

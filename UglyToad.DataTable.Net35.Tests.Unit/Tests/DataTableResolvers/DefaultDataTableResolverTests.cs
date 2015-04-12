namespace UglyToad.DataTable.Net35.Tests.Unit.Tests.DataTableResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DataTableResolver;
    using DataTypeConverter;
    using Factories;
    using Helpers;
    using POCOs;
    using TestStubs;
    using Types;
    using Xunit;
    using Xunit.Extensions;

    public class DefaultDataTableResolverTests
    {
        private readonly IDataTypeConverter dataTypeConverter = new TestConverter();
        private readonly DefaultDataTableResolver dataTableResolver = new DefaultDataTableResolver();
        private readonly DataTableParserSettings dataTableParserSettings = new DataTableParserSettings();

        [Fact]
        public void ToObjects_NullDataTable_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(null, dataTypeConverter, CreateEmptyPropertyMappings(), dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullDataTypeConverter_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(DataTableFactory.RowsForTable(), null, CreateEmptyPropertyMappings(), dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullMappings_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(DataTableFactory.RowsForTable(), dataTypeConverter, null, dataTableParserSettings));
        }

        [Fact]
        public void ToObjects_NullArguments_ThrowsException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(null, null, null, null));
        }

        [Fact]
        public void ToObjects_EmptyDataTable_ReturnsEmptyEnumerableOfCorrectType()
        {
            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(DataTableFactory.RowsForTable(), dataTypeConverter, CreateEmptyPropertyMappings(), dataTableParserSettings);

            Assert.Equal(0, results.Count());
        }

        [Fact]
        public void ToObjects_DataTableWithOneRowCorrectTypes_ReturnsEnumerableWithCorrectResult()
        {
            var mappings = MappingHelper.CreatePropertyMappingsDirectlyMatchingObject<SimpleNoIdNoAttributes>();

            DataTable dt = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<SimpleNoIdNoAttributes>();

            foreach (var mapping in mappings)
            {
                mapping.ColumnIndex = dt.Columns.IndexOf(mapping.FieldName);
            }

            dt.Rows.Add(1, "string");

            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(DataTableFactory.RowsForTable(dt), dataTypeConverter, mappings, dataTableParserSettings);

            Assert.True(results.Count == 1);
        }

        [Theory]
        [InlineData(1, "string")]
        [InlineData(-3, "")]
        [InlineData(0, "\r\n\0")]
        [InlineData(int.MinValue, "string")]
        [InlineData(int.MaxValue, "string")]
        public void ToObjects_DataTableWithIncorrectColumnIndexButCorrectColumn_ReturnsCorrectResult(int propertyOne, string propertyTwo)
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<SimpleNoIdNoAttributes>();

            dt.Rows.Add(propertyOne, propertyTwo);

            var mappings = MappingHelper.CreatePropertyMappingsMatchingTable<SimpleNoIdNoAttributes>(dt);

            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(DataTableFactory.RowsForTable(dt), dataTypeConverter, mappings, dataTableParserSettings);

            Assert.Equal(GetAssertObject<int>(propertyOne), results.First().PropertyOne);
            Assert.Equal(GetAssertObject<string>(propertyTwo), results.First().PropertyTwo);
        }

        [Fact]
        public void ToObjects_DataTableWithManyRowsMatchingColumns_ReturnsEnumerableWithCorrectResult()
        {
            const int rows = 1000;

            List<SimpleNoIdNoAttributes> objects = new List<SimpleNoIdNoAttributes>(capacity: rows);

            for (int i = 0; i < rows; i++)
            {
                objects.Add(new SimpleNoIdNoAttributes
                    {
                        PropertyOne = i + 1,
                        PropertyTwo = "Property" + i
                    });
            }

            var mappings = MappingHelper.CreatePropertyMappingsDirectlyMatchingObject<SimpleNoIdNoAttributes>();

            DataTable dt = DataTableFactory.GenerateDataTableFilledWithObjects<SimpleNoIdNoAttributes>(objects);

            foreach (var mapping in mappings)
            {
                mapping.ColumnIndex = dt.Columns.IndexOf(mapping.FieldName);
            }

            var results = dataTableResolver.ToObjects<SimpleNoIdNoAttributes>(DataTableFactory.RowsForTable(dt), dataTypeConverter, mappings, dataTableParserSettings);

            Assert.True(results.Count == rows);
        }

        private ExtendedPropertyInfo[] CreateEmptyPropertyMappings()
        {
            return new ExtendedPropertyInfo[0];
        }

        private object GetAssertObject<T>(object field)
        {
            return dataTypeConverter.FieldToObject(field, typeof(T), dataTableParserSettings, new DbNullConverter(dataTableParserSettings));
        }
    }
}

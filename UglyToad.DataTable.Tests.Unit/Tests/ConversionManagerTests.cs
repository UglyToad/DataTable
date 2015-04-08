namespace UglyToad.DataTable.Tests.Unit.Tests
{
    using System;
    using System.Data;
    using System.Linq;
    using DataTable.MappingResolvers;
    using DataTableResolver;
    using DataTypeConverter;
    using Enums;
    using POCOs;
    using TestStubs;
    using Types;
    using Xunit;

    public class ConversionManagerTests
    {
        private DataTableParserSettings defaultSettings = new DataTableParserSettings();
        private IMappingResolver defaultMappingResolver = new TestMappingResolver();
        private IDataTableResolver defaultDataTableResolver = new TestDataTableResolver();
        private IDataTypeConverter defaultDataTypeConverter = new TestConverter();

        [Fact]
        public void ConvertToType_NullDataTableWithNullErrorSetting_ThrowsArgumentNullException()
        {
            var conversionManager = GetConversionManagerWithCustomSettings(new DataTableParserSettings
                {
                    NullInputHandling = NullInputHandling.Error
                });

            Assert.Throws<ArgumentNullException>(() => conversionManager.ConvertToType<SimpleNoIdNoAttributes>(null));
        }

        [Fact]
        public void ConvertToType_NullDataTableWithNullReturnSetting_ReturnsNull()
        {
            var conversionManager = GetConversionManagerWithCustomSettings(new DataTableParserSettings
            {
                NullInputHandling = NullInputHandling.ReturnNull
            });

            var results = conversionManager.ConvertToType<SimpleNoIdNoAttributes>(null);

            Assert.Null(results);
        }

        [Fact]
        public void ConvertToType_EmptyDataTableWithEmptyReturnSetting_ReturnsEmptyEnumerable()
        {
            var conversionManager = GetConversionManagerWithCustomSettings(new DataTableParserSettings {
                EmptyInputHandling = EmptyInputHandling.ReturnEmptyEnumerable
            });

            var results = conversionManager.ConvertToType<SimpleNoIdNoAttributes>(new DataTable());

            Assert.True(results.Count() == 0);
        }

        [Fact]
        public void ConvertToType_EmptyDataTableWithErrorSetting_ThrowsInvalidOperationException()
        {
            var conversionManager = GetConversionManagerWithCustomSettings(new DataTableParserSettings
            {
                EmptyInputHandling = EmptyInputHandling.Error
            });

            Assert.Throws<InvalidOperationException>(() => conversionManager.ConvertToType<SimpleNoIdNoAttributes>(new DataTable()));
        }

        [Fact]
        public void ConvertToType_EmptyDataTableWithNullSetting_ThrowsException()
        {
            var conversionManager = GetConversionManagerWithCustomSettings(new DataTableParserSettings
            {
                EmptyInputHandling = EmptyInputHandling.ReturnNull
            });

            var results = conversionManager.ConvertToType<SimpleNoIdNoAttributes>(new DataTable());

            Assert.Null(results);
        }

        [Fact]
        public void ConvertToType_DataTableWithRow_ReturnsList()
        {
            var conversionManager = GetDefaultconversionManager();

            var dt = new DataTable();

            dt.Columns.Add("StringColumn", typeof(string));

            dt.Rows.Add("String");

            var results = conversionManager.ConvertToType<SimpleNoIdNoAttributes>(dt);

            Assert.NotNull(results);
        }

        private ConversionManager GetDefaultconversionManager()
        {
            return new ConversionManager(defaultSettings, defaultMappingResolver, defaultDataTableResolver, defaultDataTypeConverter);
        }

        private ConversionManager GetConversionManagerWithCustomSettings(DataTableParserSettings settings)
        {
            return new ConversionManager(settings, defaultMappingResolver, defaultDataTableResolver, defaultDataTypeConverter);
        }
    }
}

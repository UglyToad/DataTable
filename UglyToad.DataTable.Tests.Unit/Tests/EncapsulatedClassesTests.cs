namespace UglyToad.DataTable.Tests.Unit.Tests
{
    using System;
    using System.Linq;
    using DataTable.MappingResolvers;
    using DataTableResolver;
    using DataTypeConverter;
    using Factories;
    using Helpers;
    using POCOs;
    using TestStubs;
    using Types;
    using Xunit;

    public class EncapsulatedClassesTests
    {
        private readonly IDataTableResolver dataTableResolver = new DefaultDataTableResolver();
        private readonly DataTableParserSettings defaultSettings = new DataTableParserSettings();
        private IDataTableResolver defaultDataTableResolver = new TestDataTableResolver();
        private IDataTypeConverter defaultDataTypeConverter = new TestConverter();
        private IMappingResolver defaultMappingResolver = new TestMappingResolver();

        [Fact]
        public void ToObjects_WithPrivateConstructor_CanMapObjects()
        {
            var guid = new Guid("07494404-072A-4BE3-962E-AA3E839AD330");

            var dataTable =
                DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<PrivateConstructorPublicProperty>();

            var dataRow = dataTable.NewRow();
            dataRow["Id"] = guid;
            dataTable.Rows.Add(dataRow);

            var mappings =
                MappingHelper.CreatePropertyMappingsMatchingTable<PrivateConstructorPublicProperty>(dataTable);

            var results = dataTableResolver.ToObjects<PrivateConstructorPublicProperty>(DataTableFactory.RowsForTable(dataTable),
                new DefaultDataTypeConverter(), mappings, defaultSettings);

            Assert.Equal(1, results.Count);
            Assert.Equal(guid, results.Single().Id);
        }

        [Fact]
        public void ToObjects_WithPublicConstructorTakingArguments_CanMapObjects()
        {
            var guid = new Guid("098DE9E5-50FF-4C8C-921D-DDAA90795A63");

            var dataTable =
                DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<PublicConstructorTakingArguments>();

            var dataRow = dataTable.NewRow();

            dataRow["Id"] = guid;
            dataRow["ResolutionDetails"] = "Fixed the problem";

            dataTable.Rows.Add(dataRow);

            var mappings =
                MappingHelper.CreatePropertyMappingsMatchingTable<PublicConstructorTakingArguments>(dataTable);

            var results = dataTableResolver.ToObjects<PublicConstructorTakingArguments>(DataTableFactory.RowsForTable(dataTable),
                new DefaultDataTypeConverter(), mappings, defaultSettings);

            Assert.Equal(1, results.Count(r => r.Id == guid && r.ResolutionDetails == "Fixed the problem"));
        }

        [Fact]
        public void ToObjects_WithInternalConstructor_CanMapObjects()
        {
            var id = 15447;
            var dateTime = new DateTime(2001, 1, 1).ToShortDateString();

            var dataTable = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<InternalConstructor>();

            var dataRow = dataTable.NewRow();

            dataRow["Id"] = id;
            dataRow["ConstructionDate"] = dateTime;

            dataTable.Rows.Add(dataRow);

            var mappings = MappingHelper.CreatePropertyMappingsMatchingTable<InternalConstructor>(dataTable);

            var results = dataTableResolver.ToObjects<InternalConstructor>(DataTableFactory.RowsForTable(dataTable), new DefaultDataTypeConverter(),
                mappings, defaultSettings);

            Assert.Equal(1, results.Count(r => r.Id == id && r.ConstructionDate == DateTime.Parse(dateTime)));
        }

        [Fact]
        public void ToObjects_WithProtectedConstructor_CanMapObjects()
        {
            const int id = 15447;
            const long longValue = 6546545665446556;
            
            var dataTable = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<ProtectedConstructor>();

            var dataRow = dataTable.NewRow();

            dataRow["Id"] = id;
            dataRow["Long"] = longValue;

            dataTable.Rows.Add(dataRow);

            var mappings = MappingHelper.CreatePropertyMappingsMatchingTable<ProtectedConstructor>(dataTable);

            var results = dataTableResolver.ToObjects<ProtectedConstructor>(DataTableFactory.RowsForTable(dataTable), new DefaultDataTypeConverter(),
                mappings, defaultSettings);

            Assert.Equal(1, results.Count(r => r.Id == id && r.Long == longValue));
        }

        [Fact]
        public void ToObjects_WithPrivateSetters_CanMapObjects()
        {
            var id = "50 X0001RM";
            var decimalValue = (decimal) 0.05;

            var dataTable = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<PrivateSetters>();

            var dataRow = dataTable.NewRow();

            dataRow["Id"] = id;
            dataRow["Decimal"] = decimalValue;

            dataTable.Rows.Add(dataRow);

            var mappings = MappingHelper.CreatePropertyMappingsMatchingTable<PrivateSetters>(dataTable);

            var results = dataTableResolver.ToObjects<PrivateSetters>(DataTableFactory.RowsForTable(dataTable), new DefaultDataTypeConverter(),
                mappings, defaultSettings);

            Assert.Equal(1, results.Count(r => r.Id == id && r.Decimal == decimalValue));
        }

        [Fact]
        public void ToObjects_WithMixedSetters_CanMapObjects()
        {
            var id = 65446468;
            var color = "#333";

            var dataTable = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<MixedSetters>();

            var dataRow = dataTable.NewRow();

            dataRow["Id"] = id;
            dataRow["Color"] = color;

            dataTable.Rows.Add(dataRow);

            var mappings = MappingHelper.CreatePropertyMappingsMatchingTable<MixedSetters>(dataTable);

            var results = dataTableResolver.ToObjects<MixedSetters>(DataTableFactory.RowsForTable(dataTable), new DefaultDataTypeConverter(), mappings,
                defaultSettings);

            Assert.Equal(1, results.Count(r => r.Id == id && r.Color == color));
        }

        [Fact]
        public void GetPropertyMappings_WithPrivateSetters_GetsPrivateMap()
        {
            var id = "60 Y1100TS";
            var decimalValue = (decimal) 0.52;

            var mappingResolver = new DefaultMappingResolver();

            var dataTable = DataTableFactory.GenerateEmptyDataTableMatchingObjectProperties<PrivateSetters>();

            var dataRow = dataTable.NewRow();
            dataRow["Id"] = id;
            dataRow["Decimal"] = decimalValue;
            dataTable.Rows.Add(dataRow);

            var results = mappingResolver.GetPropertyMappings<PrivateSetters>(dataTable, defaultSettings);

            var objects = dataTableResolver.ToObjects<PrivateSetters>(DataTableFactory.RowsForTable(dataTable), new DefaultDataTypeConverter(), results,
                defaultSettings);

            Assert.Equal(1, objects.Count(ps => ps.Id == id && ps.Decimal == decimalValue));
        }
    }
}
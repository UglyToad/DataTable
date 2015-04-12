namespace UglyToad.DataTable.Net35.Tests.Unit.Tests.MappingResolvers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DataTable.MappingResolvers;
    using Enums;
    using Exceptions;
    using Factories;
    using POCOs;
    using Types;
    using Xunit;
    using Xunit.Extensions;

    public class DefaultMappingResolverTests
    {
        DefaultMappingResolver defaultMappingResolver = new DefaultMappingResolver();
        DataTableParserSettings defaultDataTableParserSettings = new DataTableParserSettings();

        [Fact]
        public void GetPropertyMappings_NullDataTable_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoAttributes>(null, defaultDataTableParserSettings));
        }

        [Fact]
        public void GetPropertyMappings_NullSettings_ThrowsArgumentNullException()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns();

            Assert.Throws(typeof(ArgumentNullException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoAttributes>(dt, null));
        }

        [Fact]
        public void GetPropertyMappings_NullSettingsAndDataTable_ThrowsArgumentNullException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoAttributes>(null, null));
        }

        [Fact]
        public void GetPropertyMappings_DataTableWithoutColumns_ReturnsEmptyCollection()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns();

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_DataTableWithUnnamedColumns_ReturnsEmptyCollection()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(null);

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_MappedClassHasNoProperties_ReturnsEmptyCollection()
        {
            string anyName = "random name";

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(anyName);

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleNoProperties>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 0);
        }

        [Fact]
        public void GetPropertyMappings_OnePropertyWithMatchingColumn_ReturnsCollectionWithOneResult()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 1);
        }

        [Fact]
        public void GetPropertyMappings_OnePropertyWithMatchingColumn_ReturnsCollectionWithCorrectResult()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoAttributes>(dt, defaultDataTableParserSettings);

            ExtendedPropertyInfo propInfo = results.First();

            Assert.True(propInfo.FieldName == "PropertyOne");

            Assert.True(propInfo.ColumnIndex == 0);
        }

        [Theory, 
        InlineData("propertyOnE"), 
        InlineData("PROPERTYONE"), 
        InlineData("propertyone")]
        public void GetPropertyMappings_OnePropertyWithMatchingColumnIncorrectCase_ReturnsCollectionWithOneResult(string columnName)
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columnName);

            ICollection<ExtendedPropertyInfo> results = defaultMappingResolver.GetPropertyMappings<SimpleOnePropertyNoIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Count == 1);
        }

        [Theory, 
        InlineData("propertyOne", "PropertyTwo"), 
        InlineData("PropertyOne", "PropertyTwo"), 
        InlineData("PropertyTwo", "PropertyOne")]
        public void GetPropertyMappings_TwoPropertiesWithMatchingColumns_ReturnsCollectionWithTwoResults(string column1, string column2)
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(column1, column2);

            var results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Length == 2);
        }

        [Fact]
        public void GetPropertyMappings_TwoPropertiesOneMatching_ThrowsMissingMappingException()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            DataTableParserSettings dtps = new DataTableParserSettings { MissingMappingHandling = MissingMappingHandling.Error };

            Assert.Throws(typeof(MissingMappingException<SimpleNoIdNoAttributes>), () => defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoAttributes>(dt, dtps));
        }

        [Fact]
        public void GetPropertyMappings_TwoPropertiesOneMatching_MissingMappingHandlingIgnores()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("PropertyOne");

            DataTableParserSettings dtps = new DataTableParserSettings { MissingMappingHandling = MissingMappingHandling.Ignore };

            var results = defaultMappingResolver.GetPropertyMappings<SimpleNoIdNoAttributes>(dt, dtps);

            Assert.True(results.Length == 1);
        }

        [Fact]
        public void GetPropertyMappings_ClassWithInheritedProperties_ReturnsAllResults()
        {
            string[] columns = { "ParentIntProperty", "ParentStringProperty", "ChildIntProperty", "ChildStringProperty" };

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columns);

            DataTableParserSettings dtps = new DataTableParserSettings {  InheritMappings = true };

            var results = defaultMappingResolver.GetPropertyMappings<ChildNoAttributes>(dt, dtps);

            Assert.True(results.Length == 4);
        }

        [Fact]
        public void GetPropertyMappings_ClassWithTwoLevelsOfInheritance_ReturnsAllResults()
        {
            string[] columns = { "ParentIntProperty", "ParentStringProperty", "ChildIntProperty", "ChildStringProperty", "LeafIntProperty" };

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columns);

            DataTableParserSettings dtps = new DataTableParserSettings { InheritMappings = true };

            var results = defaultMappingResolver.GetPropertyMappings<LeafClassNoAttributes>(dt, dtps);

            Assert.True(results.Length == 5);
        }

        [Fact]
        public void GetPropertyMappings_ClassWithInheritedProperties_OnlyReturnsImmediateProperties()
        {
            string[] columns = { "ParentIntProperty", "ParentStringProperty", "ChildIntProperty", "ChildStringProperty" };

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columns);

            DataTableParserSettings dtps = new DataTableParserSettings { InheritMappings = false };

            var results = defaultMappingResolver.GetPropertyMappings<ChildNoAttributes>(dt, dtps);

            Assert.True(results.Length == 2);
        }

        [Fact]
        public void GetPropertyMappings_OneIdPropertyMatchingMapping_ReturnsResults()
        {
            string[] columns = { "Id", "PropertyOne" };

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columns);

            var results = defaultMappingResolver.GetPropertyMappings<SimpleIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Length == 2);

            Assert.True(results.Select(r => r.FieldName).Contains("Id", StringComparer.InvariantCultureIgnoreCase));
        }

        [Fact]
        public void GetPropertyMappings_OneIdPropertyMatchingMappingCaseVaries_ReturnsResults()
        {
            string[] columns = { "iD", "PropertyOne" };

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columns);

            var results = defaultMappingResolver.GetPropertyMappings<SimpleIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Length == 2);

            Assert.True(results.Select(r => r.FieldName).Contains("id", StringComparer.InvariantCultureIgnoreCase));
        }

        [Fact]
        public void GetPropertyMappings_OneIdClassNameMapping_ReturnsResults()
        {
            string[] columns = { typeof(SimpleIdNoAttributes).Name + "Id", "PropertyOne" };

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columns);

            var results = defaultMappingResolver.GetPropertyMappings<SimpleIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Length == 2);

            Assert.True(results.Select(r => r.PropertyInfo.Name).Contains("Id", StringComparer.InvariantCultureIgnoreCase));
        }

        [Fact]
        public void GetPropertyMappings_OneIdClassNameMappingCaseVaries_ReturnsResults()
        {
            string[] columns = { typeof(SimpleIdNoAttributes).Name + "iD", "PropertyOne" };

            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns(columns);

            var results = defaultMappingResolver.GetPropertyMappings<SimpleIdNoAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Length == 2);

            Assert.True(results.Select(r => r.PropertyInfo.Name).Contains("Id", StringComparer.InvariantCultureIgnoreCase));
        }

        [Fact]
        public void GetPropertyMappings_TwoAttributesWithMatchingColumns_ReturnsAllResults()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("Prop1", "Prop2");

            var results = defaultMappingResolver.GetPropertyMappings<SimpleClassWithAttributes>(dt, defaultDataTableParserSettings);

            Assert.True(results.Length == 2);
        }

        [Fact]
        public void GetPropertyMappings_TwoAttributesWithOneMatchingColumnIgnoreProperties_ReturnsOneResult()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("prop1", "PropertyTwo");

            DataTableParserSettings dtps = new DataTableParserSettings
            {
                MissingMappingHandling = MissingMappingHandling.Ignore,
                MappingMatchOrder = MappingMatchOrder.IgnorePropertyNames
            };

            var results = defaultMappingResolver.GetPropertyMappings<SimpleClassWithAttributes>(dt, dtps);

            Assert.True(results.Length == 1);
        }

        [Fact]
        public void GetPropertyMappings_TwoAttributesWithOneMatchingColumnErrorOnMissingMapping_ThrowsMissingMappingException()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("prop1", "PropertyTwo");

            DataTableParserSettings dtps = new DataTableParserSettings
            {
                MissingMappingHandling = MissingMappingHandling.Error,
                MappingMatchOrder = MappingMatchOrder.IgnorePropertyNames
            };

            Assert.Throws(typeof(MissingMappingException<SimpleClassWithAttributes>), () => defaultMappingResolver.GetPropertyMappings<SimpleClassWithAttributes>(dt, dtps));
        }

        [Fact]
        public void GetPropertyMappings_TwoAttributesWithOneMatchingColumnAndPropertyMatch_ReturnsResults()
        {
            DataTable dt = DataTableFactory.GenerateEmptyDataTableWithStringColumns("prop1", "PropertyTwo");

            DataTableParserSettings dtps = new DataTableParserSettings
            {
                MissingMappingHandling = MissingMappingHandling.Error,
                MappingMatchOrder = MappingMatchOrder.PropertyNameFirst
            };

            var results = defaultMappingResolver.GetPropertyMappings<SimpleClassWithAttributes>(dt, dtps);

            Assert.True(results.Length == 2);
        }
    }
}

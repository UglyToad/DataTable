﻿namespace UglyToad.DataTable.Tests.Unit.Tests
{
    using System;
    using System.Data;
    using System.Linq;
    using Enums;
    using Factories;
    using POCOs;
    using Types;
    using Xunit;

    public class DataTableConverterTests
    {
        private const int Count = 5000;

        private readonly Func<int, int> idForIndex = i => i * 2;
        private readonly Func<int, int> addressIdForIndex = i => i + 1;
        
            [Fact]
        public void Convert_CanMapIdColumnsCorrectly()
        {
            var dataTable = DataTableForIdClasses();

            var results = DataTableConverter.Convert<TwoIdProperties>(dataTable);

            Assert.Equal(Count, results.Count);
        }

        [Fact]
        public void Convert_CanMapIdColumnsWithAttributesCorrectly()
        {
            var dataTable = DataTableForIdClasses();

            var results = DataTableConverter.Convert<TwoIdPropertiesWithAttribute>(dataTable);

            Assert.Equal(Count, results.Count);
            Assert.Equal(idForIndex(7), results[7].Id);
            Assert.Equal(addressIdForIndex(7), results[7].AddressRef);
        }

        [Fact]
        public void Convert_CanMapSpecificIdColumnsCorrectly()
        {
            var dataTable = DataTableForIdClasses();

            var results = DataTableConverter.Convert<TwoSpecificIdPropertiesWithAttribute>(dataTable);

            Assert.Equal(idForIndex(7), results[7].TwoSpecificIdPropertiesWithAttributeId);
            Assert.Equal(addressIdForIndex(7), results[7].AddressId);
        }

        [Fact]
        public void ToObjects_CanConvertSimpleClass()
        {
            const int id = 598897;
            const string propertyOne = "Vitamin";

            var data = new[]
            {
                new SimpleIdNoAttributes
                {
                    Id = id,
                    PropertyOne = propertyOne
                }
            };

            var dataTable = DataTableFactory.GenerateDataTableFilledWithObjects(data);

            var objects = DataTableConverter.Create().ConvertToObjectList<SimpleIdNoAttributes>(dataTable);
            
            Assert.Equal(id, objects.Single().Id);
            Assert.Equal(propertyOne, objects.Single().PropertyOne);
        }

        [Fact]
        public void ConvertToObjectList_ListOfMixedAttributePropertyClass_ConvertsWithOrder()
        {
            var dataTable = GetDataTableForClassWithSomeAttributes();

            var results = DataTableConverter.Convert<ClassWithSomeAttributes>(dataTable);

            int[] ints = new int[Count];

            for (int i = 0; i < Count; i++)
            {
                ints[i] = i;
            }

            Assert.Equal(ints, results.Select(r => r.Count));
        }

        [Fact]
        public void ConvertToObjectList_SetDelegateResolver_ConvertsWithOrder()
        {
            var dataTable = GetDataTableForClassWithSomeAttributes();

            var converter = DataTableConverter.Create(new DataTableParserSettings
            {
                Resolver = Resolver.Delegate
            });

            var results = converter.ConvertToObjectList<ClassWithSomeAttributes>(dataTable);

            int[] ints = new int[Count];

            for (int i = 0; i < Count; i++)
            {
                ints[i] = i;
            }

            Assert.Equal(ints, results.Select(r => r.Count));
        }

        [Fact]
        public void ConvertToObjectList_DefaultResolver_ConvertsCorrectly()
        {
            var dataTable = GetDataTableForClassWithSomeAttributes();

            var converter = DataTableConverter.Create(new DataTableParserSettings
            {
                Resolver = Resolver.Delegate
            });

            var results = converter.ConvertToObjectList<ClassWithSomeAttributes>(dataTable);

            string[] owners = new string[Count];

            for (int i = 0; i < Count; i++)
            {
                owners[i] = "Lord Beekeeper The " + i;
            }

            Assert.Equal(owners, results.Select(r => r.Owner));
        }

        [Fact]
        public void Convert_AttributeOrPropertyName_MapCorrectly()
        {
            var dataTable = GetDataTableForClassWithSomeAttributes();

            var resultsUsingAttribute = DataTableConverter.Convert<ClassWithSomeAttributes>(dataTable);

            dataTable.Columns["Beehive"].ColumnName = "Count";

            var resultsUsingProperty = DataTableConverter.Convert<ClassWithSomeAttributes>(dataTable);

            Assert.Equal(resultsUsingAttribute.Select(r => r.Count), resultsUsingProperty.Select(r => r.Count));
        }

        [Fact]
        public void Convert_TwoAttributes_MapCorrectly()
        {
            var dataTable = GetDataTableForClassWithSomeAttributes();

            var resultsUsingAttribute = DataTableConverter.Convert<ClassWithSomeAttributes>(dataTable);

            dataTable.Columns["Beehive"].ColumnName = "beehive_count";

            var resultsUsingSecondAttribute = DataTableConverter.Convert<ClassWithSomeAttributes>(dataTable);

            Assert.Equal(resultsUsingAttribute.Select(r => r.Count), resultsUsingSecondAttribute.Select(r => r.Count));
        }

        private DataTable GetDataTableForClassWithSomeAttributes()
        {
            var dataTable = new DataTable();

            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("Beehive", typeof (string)),
                new DataColumn("Owner", typeof (string)),
                new DataColumn("Foundation", typeof (DateTime)),
                new DataColumn("Viable", typeof (int))
            });

            for (int i = 0; i < Count; i++)
            {
                var dataRow = dataTable.NewRow();
                
                dataRow["Beehive"] = i.ToString();
                dataRow["Owner"] = "Lord Beekeeper The " + i;
                dataRow["Foundation"] = new DateTime(2001, 1, 1);
                dataRow["Viable"] = (i % 2 == 0) ? 1 : 0;

                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
        
        private DataTable DataTableForIdClasses()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof (int));
            dataTable.Columns.Add("AddressId", typeof (short));

            for (int i = 0; i < Count; i++)
            {
                var dataRow = dataTable.NewRow();

                dataRow["Id"] = idForIndex(i);
                dataRow["AddressId"] = addressIdForIndex(i);

                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
    }
}

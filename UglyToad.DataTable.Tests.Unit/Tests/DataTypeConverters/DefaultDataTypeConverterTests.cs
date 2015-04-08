namespace UglyToad.DataTable.Tests.Unit.Tests.DataTypeConverters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DataTypeConverter;
    using Types;
    using Xunit;

    public class DefaultDataTypeConverterTests
    {
        private DataTableParserSettings defaultSettings = new DataTableParserSettings();

        public static IEnumerable<object[]> StringTestData
        {
            get
            {
                return new[]
                {
                    new object[] { string.Empty },
                    new object[] { null },
                    new object[] { 'x' },
                    new object[] { '\r' },
                    new object[] { "string" },
                    new object[] { new DateTime(2001, 1, 1) },
                    new object[] { int.MinValue },
                    new object[] { int.MaxValue },
                    new object[] { 0.5454f },
                    new object[] { DBNull.Value },
                    new object[] { Guid.Empty }
                };
            }
        }

        public static IEnumerable<object[]> UnsupportedClassTestData
        {
            get
            {
                return new[]
                {
                    new object[] { string.Empty },
                    new object[] { 'x' },
                    new object[] { '\r' },
                    new object[] { "string" },
                    new object[] { new DateTime(2001, 1, 1) },
                    new object[] { int.MinValue },
                    new object[] { int.MaxValue },
                    new object[] { 0.5454f },
                    new object[] { Guid.Empty }
                };
            }
        }

        public static IEnumerable<object[]> IntTestDataGood
        {
            get
            {
                return new[]
                {
                    new object[] { int.MinValue },
                    new object[] { int.MaxValue },
                    new object[] { 0 },
                    new object[] { "1" },
                    new object[] { "-1" },
                    new object[] { 1.0 },
                    new object[] { 1.3 }
                };
            }
        }

        public static IEnumerable<object[]> IntTestDataBad
        {
            get
            {
                return new[]
                {
                    new object[] { "az" },
                    new object[] { new DateTime(2001, 1, 1) },
                    new object[] { Guid.Empty }
                };
            }
        }

        [Theory]
        [MemberData("StringTestData")]
        public void FieldToObject_TypeToString_ReturnsString(object input)
        {
            DefaultDataTypeConverter converter = new DefaultDataTypeConverter();

            object toConvert = input;

            object result = converter.FieldToObject(toConvert, typeof(string), defaultSettings, new DbNullConverter(defaultSettings));

            string expectedResult = (input == null || input == DBNull.Value) ? null : input.ToString();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData("UnsupportedClassTestData")]
        public void FieldToObject_TypeToUnsupportedClass_ThrowsNotImplementedException(object input)
        {
            var converter = new DefaultDataTypeConverter();

            Assert.Throws<NotImplementedException>(() => converter.FieldToObject(input, typeof(StringBuilder), defaultSettings, new DbNullConverter(defaultSettings)));
        }

        [Theory]
        [MemberData("IntTestDataGood")]
        public void FieldToObject_SupportedTypeToInt_ReturnsInt(object toConvert)
        {
            var converter = new DefaultDataTypeConverter();

            object result = converter.FieldToObject(toConvert, typeof(int), defaultSettings, new DbNullConverter(defaultSettings));

            int expectedResult = Convert.ToInt32(toConvert);

            Assert.Equal(expectedResult, (int)result);
        }

        [Theory]
        [MemberData("IntTestDataBad")]
        public void FieldToObject_UnsupportedTypeToInt_ThrowsNotImplementedException(object toConvert)
        {
            var converter = new DefaultDataTypeConverter();

            Assert.Throws<NotImplementedException>(() => converter.FieldToObject(toConvert, typeof(int), defaultSettings, new DbNullConverter(defaultSettings)));
        }
    }
}

namespace UglyToad.DataTable.Net35.Tests.Unit.Tests.DataTypeConverters
{
    using System;
    using DataTypeConverter;
    using Types;
    using Xunit;
    using Xunit.Extensions;

    public class DefaultDataTypeConverterTests
    {
        private readonly DataTableParserSettings defaultSettings = new DataTableParserSettings();

        [Theory]
        [InlineData(null)]
        [InlineData('x')]
        [InlineData('\r')]
        [InlineData("string")]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(0.5454f)]
        public void FieldToObject_TypeToString_ReturnsString(object input)
        {
            DefaultDataTypeConverter converter = new DefaultDataTypeConverter();

            object toConvert = input;

            object result = converter.FieldToObject(toConvert, typeof(string), defaultSettings, new DbNullConverter(defaultSettings));

            string expectedResult = (input == null || input == DBNull.Value) ? null : input.ToString();

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(0)]
        [InlineData("1")]
        [InlineData("-1")]
        [InlineData(1.0)]
        [InlineData(1.3)]
        public void FieldToObject_SupportedTypeToInt_ReturnsInt(object toConvert)
        {
            var converter = new DefaultDataTypeConverter();

            object result = converter.FieldToObject(toConvert, typeof(int), defaultSettings, new DbNullConverter(defaultSettings));

            int expectedResult = Convert.ToInt32(toConvert);

            Assert.Equal(expectedResult, (int)result);
        }

        [Theory]
        [InlineData("az")]
        [InlineData("70FC0383-21CB-4E4E-8846-A6C4E6F0DA83")]
        public void FieldToObject_UnsupportedTypeToInt_ThrowsNotImplementedException(object toConvert)
        {
            var converter = new DefaultDataTypeConverter();

            Assert.Throws<NotImplementedException>(() => converter.FieldToObject(toConvert, typeof(int), defaultSettings, new DbNullConverter(defaultSettings)));
        }
    }
}

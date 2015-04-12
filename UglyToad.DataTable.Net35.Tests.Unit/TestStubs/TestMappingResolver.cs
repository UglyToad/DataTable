namespace UglyToad.DataTable.Net35.Tests.Unit.TestStubs
{
    using System.Collections.Generic;
    using System.Data;
    using MappingResolvers;
    using Types;

    internal class TestMappingResolver : IMappingResolver
    {
        private ICollection<ExtendedPropertyInfo> mappings;

        public TestMappingResolver()
        {
        }

        public TestMappingResolver(ICollection<ExtendedPropertyInfo> mappings)
        {
            this.mappings = mappings;
        }

        public ExtendedPropertyInfo[] GetPropertyMappings<T>(DataTable dataTable, DataTableParserSettings settings)
        {
            return new ExtendedPropertyInfo[] { };
        }
    }
}

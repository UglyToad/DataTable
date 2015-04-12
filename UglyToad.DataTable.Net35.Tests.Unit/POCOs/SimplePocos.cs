namespace UglyToad.DataTable.Net35.Tests.Unit.POCOs
{
    internal class TwoIdProperties
    {
        public int Id { get; set; }

        public int AddressId { get; set; }
    }

    internal class SimpleNoIdNoAttributes
    {
        public int PropertyOne { get; set; }

        public string PropertyTwo { get; set; }
    }

    internal class SimpleNoIdNoAttributesStringsOnly
    {
        public string PropertyOne { get; set; }

        public string PropertyTwo { get; set; }
    }

    internal class SimpleNoProperties
    {
        public int PropertyOne = 0;
    }

    internal class SimpleOnePropertyNoIdNoAttributes
    {
        public int PropertyOne { get; set; }
    }

    internal class SimpleIdNoAttributes
    {
        public int Id { get; set; }

        public string PropertyOne { get; set; }
    }
}

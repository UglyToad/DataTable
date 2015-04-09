namespace UglyToad.DataTable.Tests.Unit.POCOs
{
    using System;
    using Types;

    internal class SimpleClassWithAttributes
    {
        [ColumnMapping(Name = "Prop1")]
        public int PropertyOne { get; set; }

        [ColumnMapping(Name = "Prop2")]
        public string PropertyTwo { get; set; }
    }

    internal class SimpleClassWithAttributesAndId
    {
        [ColumnMapping(Name = "class_id")]
        public int Id { get; set; }

        [ColumnMapping(Name = "PropOne")]
        public string PropertyOne { get; set; }
    }

    internal class ClassWithSomeAttributes
    {
        [ColumnMapping("beehive_count")]
        [ColumnMapping(Name = "Beehive")]
        public int Count { get; set; }

        public string Owner { get; set; }

        public DateTime Foundation { get; set; }

        [ColumnMapping("Viable")]
        public bool HasQueen { get; set; }
    }
}

namespace UglyToad.DataTable.Tests.Unit.POCOs
{
    internal class ParentNoAttributes
    {
        public int ParentIntProperty { get; set; }

        public string ParentStringProperty { get; set; }
    }

    internal class ChildNoAttributes  : ParentNoAttributes
    {
        public int ChildIntProperty { get; set; }

        public string ChildStringProperty { get; set; }
    }

    internal class LeafClassNoAttributes : ChildNoAttributes
    {
        public int LeafIntProperty { get; set; }
    }
}

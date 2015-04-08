namespace UglyToad.DataTable.Types
{
    using System;

    public class ColumnMapping : Attribute
    {
        public string Name { get; set; }

        public ColumnMapping(string name)
        {
            Name = name;
        }

        public ColumnMapping()
        {    
        }
    }
}

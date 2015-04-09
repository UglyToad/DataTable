namespace UglyToad.DataTable.Types
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
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

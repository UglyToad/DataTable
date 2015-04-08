namespace UglyToad.DataTable.Types
{
    using System.Reflection;

    public class ExtendedPropertyInfo
    {
        public string FieldName { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public int ColumnIndex { get; set; }

        public ExtendedPropertyInfo(string fieldName, PropertyInfo propertyInfo, int columnIndex)
        {
            this.FieldName = fieldName;
            this.PropertyInfo = propertyInfo;
            this.ColumnIndex = columnIndex;
        }
    }
}

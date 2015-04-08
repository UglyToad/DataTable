namespace UglyToad.DataTable.Enums
{
    /// <summary>
    /// Sets whether the property name or the attribute value should take precedence in mapping columns to properties.
    /// </summary>
    public enum MappingMatchOrder
    {
        PropertyNameFirst = 1,
        AttributeValueFirst = 2,
        IgnorePropertyNames = 3,
        IgnoreAttributes = 4
    }
}

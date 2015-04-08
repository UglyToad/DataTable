namespace UglyToad.DataTable.Enums
{
    /// <summary>
    /// Handles how the parser will react when not all object properties are in the <see cref="DataTable"/>
    /// </summary>
    public enum MissingMappingHandling
    {
        Error = 1,
        Ignore = 2
    }
}

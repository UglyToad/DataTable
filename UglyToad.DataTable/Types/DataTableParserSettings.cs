namespace UglyToad.DataTable.Types
{
    using Enums;

    /// <summary>
    /// Settings for de-serializing a DataTable.
    /// </summary>
    public class DataTableParserSettings
    {
        private MissingMappingHandling missingMappingHandling = MissingMappingHandling.Ignore;
        private MappingMatchOrder mappingMatchOrder = MappingMatchOrder.PropertyNameFirst;
        private NullInputHandling nullInputHandling = NullInputHandling.ReturnNull;
        private EmptyInputHandling emptyInputHandling = EmptyInputHandling.ReturnEmptyEnumerable;
        private bool subsequentMappingsShouldOverwrite;
        private bool allowDuplicateMappings;
        private bool inheritMappings = true;
        private bool allowDbNullForNonNullableTypes = true;
        private Resolver resolver = Resolver.Default;

        /// <summary>
        /// Gets or sets whether the mappings that come after the first set (property after attribute or vice versa) should
        /// overwrite the previous mappings if both map. False by default.
        /// </summary>
        public bool SubsequentMappingsShouldOverwrite 
        {
            get { return subsequentMappingsShouldOverwrite; }
            set { subsequentMappingsShouldOverwrite = value; }
        }

        public bool AllowDbNullForNonNullableTypes 
        {
            get { return allowDbNullForNonNullableTypes; }
            set { allowDbNullForNonNullableTypes = value; } 
        }

        /// <summary>
        /// Gets or sets whether two properties should be allowed to map to the same column. False by default.
        /// </summary>
        public bool AllowDuplicateMappings 
        {
            get { return allowDuplicateMappings; }
            set { allowDuplicateMappings = value; }
        }

        /// <summary>
        /// Gets or sets the handling of an incomplete DataTable to Class mapping.
        /// </summary>
        public MissingMappingHandling MissingMappingHandling 
        {
            get { return missingMappingHandling; }
            set { missingMappingHandling = value; } 
        }

        /// <summary>
        /// Gets or sets the order of importance with which property and attribute names are treated.
        /// </summary>
        public MappingMatchOrder MappingMatchOrder 
        {
            get { return mappingMatchOrder; }
            set { mappingMatchOrder = value; } 
        }

        /// <summary>
        /// Gets or sets the handling of null DataTables.
        /// </summary>
        public NullInputHandling NullInputHandling
        {
            get { return nullInputHandling; }
            set { nullInputHandling = value; }
        }

        /// <summary>
        /// Gets or sets the handling of empty DataTables.
        /// </summary>
        public EmptyInputHandling EmptyInputHandling
        {
            get { return emptyInputHandling; }
            set { emptyInputHandling = value; }
        }

        /// <summary>
        /// Gets or sets the handling of inherited properties. True by default.
        /// </summary>
        public bool InheritMappings 
        {
            get { return inheritMappings; }
            set { inheritMappings = value; }
        }

        public Resolver Resolver
        {
            get { return resolver; }
            set { resolver = value; } 
        }
    }
}

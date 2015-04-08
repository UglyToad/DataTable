namespace UglyToad.DataTable.Tests.Unit.POCOs
{
    using System;

    public class PrivateConstructorPublicProperty
    {
        public Guid Id { get; set; }

        private PrivateConstructorPublicProperty()
        {
        }

        public PrivateConstructorPublicProperty(Guid id)
        {
            this.Id = id;
        }
    }

    public class PublicConstructorTakingArguments
    {
        public Guid Id { get; set; }

        public string ResolutionDetails { get; set; }

        public PublicConstructorTakingArguments(Guid id, string resolutionDetails)
        {
            this.Id = id;
            this.ResolutionDetails = resolutionDetails;
        }
    }

    public class InternalConstructor
    {
        public int Id { get; set; }

        public DateTime ConstructionDate { get; set; }

        internal InternalConstructor()
        {
        }
    }

    public class ProtectedConstructor
    {
        public int Id { get; set; }

        public long Long { get; set; }

        protected ProtectedConstructor(int id, long longParam)
        {
            this.Id = id;
            this.Long = longParam;
        }
    }

    public class PrivateSetters
    {
        public string Id { get; private set; }

        public decimal Decimal { get; private set; }
    }

    public class MixedSetters
    {
        public int Id { get; private set; }

        public string Color { get; set; }
    }
}

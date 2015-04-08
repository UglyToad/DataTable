namespace UglyToad.DataTable.Tests.Integration.Entities
{
    public class StatusPropertyNamesMatch
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }
    }

    public class StatusPropertyNameMissing
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class StatusExtraProperty
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Extra { get; set; }
    }
}

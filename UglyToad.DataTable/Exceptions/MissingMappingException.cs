namespace UglyToad.DataTable.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MissingMappingException<T> : Exception
    {
        public MissingMappingException()
            : this("Missing Mapping for Type: " + typeof(T).Name)
        {
        }

        public MissingMappingException(string message)
            : base(message)
        {
        }

        public MissingMappingException(string message, Exception inner) :
            base(message, inner)
        {
        }

        protected MissingMappingException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }
    }
}

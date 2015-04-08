namespace UglyToad.DataTable.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidMappingException<T> : Exception
    {
        public InvalidMappingException()
            : this("Invalid Mapping for Type: " + typeof(T).Name)
        {
        }

        public InvalidMappingException(string message)
            : base(message)
        {
        }

        public InvalidMappingException(string message, Exception inner) :
            base(message, inner)
        {
        }

        protected InvalidMappingException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }
    }
}

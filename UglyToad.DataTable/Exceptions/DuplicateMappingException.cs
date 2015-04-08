namespace UglyToad.DataTable.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DuplicateMappingException<T> : Exception
    {
        public DuplicateMappingException()
            : this("Duplicate Mapping for Type: " + typeof(T).Name)
        {
        }

        public DuplicateMappingException(string message)
            : base(message)
        {
        }

        public DuplicateMappingException(string message, Exception inner) :
            base(message, inner)
        {
        }

        protected DuplicateMappingException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }
    }
}

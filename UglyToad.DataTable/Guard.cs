namespace UglyToad.DataTable
{
    using System;

    internal static class Guard
    {
        public static void ArgumentNotNull<T>(T argument) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(message: "Null parameter of type: " + typeof(T).Name, innerException: null);
            }
        }
    }
}

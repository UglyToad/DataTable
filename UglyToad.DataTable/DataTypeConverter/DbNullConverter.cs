namespace UglyToad.DataTable.DataTypeConverter
{
    using System;
    using Types;

    public class DbNullConverter
    {
        private DataTableParserSettings settings;

        public DbNullConverter(DataTableParserSettings settings)
        {
            this.settings = settings;
        }

        public virtual object DbNullToObject(Type targetReturnType)
        {
            if (IsNullable(targetReturnType))
            {
                return null;
            }

            if (targetReturnType.IsValueType)
            {
                return HandleNullValueType(targetReturnType);
            }

            throw new NotImplementedException("Attempting to map DbNull to : " + targetReturnType.Name);
        }

        protected virtual bool IsNullable(Type type)
        {
            // TODO: May be a problem if GetType() used down the stack.
            bool isNullable = (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));

            if (!isNullable)
            {
                isNullable = type.IsClass;
            }

            return isNullable;
        }

        protected virtual object HandleNullValueType(Type type)
        {
            if (!settings.AllowDbNullForNonNullableTypes)
            {
                throw new InvalidOperationException("Null field when trying to map to type: " + type.Name);
            }

            return Activator.CreateInstance(type);
        }
    }
}

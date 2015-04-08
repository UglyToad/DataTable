namespace UglyToad.DataTable.DataTypeConverter
{
    using System;
    using Types;

    public class DefaultDataTypeConverter : IDataTypeConverter
    {
        private Type stringType = typeof(string);

        public virtual object FieldToObject(object field,
            Type type,
            DataTableParserSettings settings,
            DbNullConverter dbNullConverter)
        {
            if (field == DBNull.Value || field == null)
            {
                return dbNullConverter.DbNullToObject(type);
            }

            if (type == stringType)
            {
                return field.ToString();
            }

            if (!type.IsValueType && type != stringType)
            {
                throw new NotImplementedException("No Conversion exists for class of type: " + type.Name);
            }

            return ValueTypeFieldToObject(field, type, settings);
        }

        protected virtual object ValueTypeFieldToObject(object field, Type type, DataTableParserSettings settings)
        {
            try
            {
                if (type == typeof(int))
                {
                    return FieldToInt(field);
                }
                else if (type == typeof(Guid))
                {
                    return FieldToGuid(field);
                }
                else if (type == typeof(DateTime))
                {
                    return FieldToDateTime(field);
                }
                else if (type == typeof(bool))
                {
                    return FieldToBool(field);
                }
                else if (type == typeof(char))
                {
                    return FieldToChar(field);
                }
                else if (type == typeof(decimal))
                {
                    return FieldToDecimal(field);
                }
                else if (type == typeof(float))
                {
                    return FieldToFloat(field);
                }
                else if (type == typeof(uint))
                {
                    int value = (int)FieldToInt(field);

                    if (value < 0)
                    {
                        value = value * -1;
                    }
                    return value;
                }
                else if (type == typeof(double))
                {
                    return FieldToDouble(field);
                }
                else if (type == typeof(long))
                {
                    return FieldToLong(field);
                }

                throw new NotImplementedException(string.Format("No conversion for field with value: {0} to type: {1}", field.ToString(), type.Name));
            }
            catch (InvalidCastException ex)
            {
                throw new NotImplementedException(string.Format("No conversion for field with value: {0} to type: {1}", field.ToString(), type.Name), ex);
            }
        }

        private object FieldToLong(object field)
        {
            if (field is long || field is int)
            {
                return field;
            }
            else if (field is string)
            {
                long l;

                if (long.TryParse(field.ToString(), out l))
                {
                    return l;
                }
            }

            throw new NotImplementedException(string.Format("No conversion for field with value: {0} to long", field.ToString()));
        }

        protected virtual object FieldToDateTime(object field)
        {
            if (field is DateTime)
            {
                return field;
            }
            else if (field is string)
            {
                DateTime returnDateTime;

                if (DateTime.TryParse(field.ToString(), out returnDateTime))
                {
                    return returnDateTime;
                }
                else
                {
                    throw new NotImplementedException(string.Format("No conversion for string with value: {0} to DateTime", field.ToString()));
                }
            }
            else
            {
                throw new NotImplementedException(string.Format("No conversion for field with value: {0} to DateTime", field.ToString()));
            }
        }

        protected virtual object FieldToInt(object field)
        {
            if (field is int)
            {
                return field;
            }
            else if (field is string)
            {
                int returnValue;

                bool canParse = int.TryParse(field.ToString(), out returnValue);

                if (!canParse)
                {
                    throw new NotImplementedException(string.Format("Cannot convert string: {0} to int", field.ToString()));
                }

                return returnValue;
            }
            else
            {
                return Convert.ToInt32(field);
            }
        }

        protected virtual object FieldToGuid(object field)
        {
            if (field is Guid)
            {
                return field;
            }
            else if (field is string)
            {
                return Guid.Parse(field.ToString());
            }
            else
            {
                Guid returnGuid;

                if (Guid.TryParse(field.ToString(), out returnGuid))
                {
                    return returnGuid;
                }
                else
                {
                    throw new NotImplementedException(string.Format("Cannot convert field: {0} to Guid", field.ToString()));
                }
            }
        }

        protected virtual object FieldToBool(object field)
        {
            if (field is bool)
            {
                return field;
            }
            else if (field is int)
            {
                return (int)field == 1;
            }
            else if (field is string)
            {
                bool value = string.Compare(field.ToString(), "true", true) == 0 || field.ToString() == "1";

                return value;
            }
            else
            {
                bool value;

                if (bool.TryParse(field.ToString(), out value))
                {
                    return value;
                }
                throw new NotImplementedException(string.Format("Cannot convert field: {0} to bool", field.ToString()));
            }
        }

        protected virtual object FieldToChar(object field)
        {
            if (field is char)
            {
                return field;
            }
            else
            {
                return field.ToString()[0];
            }
        }

        protected virtual object FieldToDecimal(object field)
        {
            if (field is decimal)
            {
                return field;
            }
            else if (field is float || field is double || field is int)
            {
                return (decimal)field;
            }
            else
            {
                decimal result;
                if (decimal.TryParse(field.ToString(), out result))
                {
                    return result;
                }
                throw new NotImplementedException(string.Format("Cannot convert field: {0} to decimal", field.ToString()));
            }
        }

        protected virtual object FieldToFloat(object field)
        {
            if (field is float || field is decimal || field is int)
            {
                return field;
            }
            else if (field is double)
            {
                float result = (float)field;

                if (float.IsPositiveInfinity(result) || float.IsNegativeInfinity(result))
                {
                    throw new InvalidCastException("Could not convert double to float without clipping the value");
                }

                return result;
            }
            else
            {
                float f;
                if (float.TryParse(field.ToString(), out f))
                {
                    return f;
                }
                else
                {
                    throw new NotImplementedException(string.Format("Cannot convert field: {0} to float", field.ToString()));
                }
            }
        }

        protected virtual object FieldToDouble(object field)
        {
            if (field is double 
                || field is float 
                || field is decimal 
                || field is int
                || field is short
                || field is long)
            {
                return field;
            }
            else
            {
                double d;
                if (double.TryParse(field.ToString(), out d))
                {
                    return d;
                }
                throw new NotImplementedException(string.Format("Cannot convert field: {0} to double", field.ToString()));
            }
        }
    }
}

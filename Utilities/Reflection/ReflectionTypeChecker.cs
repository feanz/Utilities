using System;
using System.Reflection;

namespace Utilities.Reflection
{
    /// <summary>
    ///   Various reflection based utility methods.
    /// </summary>
    public class ReflectionTypeChecker
    {
        /// <summary>
        ///   Checks whether or not the supplied text can be converted to a specific type.
        /// </summary>
        /// <typeparam name="T"> Type to convert to. </typeparam>
        /// <param name="val"> The value to test for conversion to the type associated with the property </param>
        /// <returns> True if </returns>
        public static bool CanConvertTo<T>(string val)
        {
            return CanConvertTo(typeof (T), val);
        }

        /// <summary>
        ///   Checks whether or not the supplied text can be converted to a specific type.
        /// </summary>
        /// <param name="type"> The type to convert val to </param>
        /// <param name="val"> The value to test for conversion to the type associated with the property </param>
        /// <returns> True if the conversion can be performed. </returns>
        public static bool CanConvertTo(Type type, string val)
        {
            // Data could be passed as string value.
            // Try to change type to check type safety.                    
            try
            {
                if (type == typeof (int))
                {
                    int result;
                    return int.TryParse(val, out result);
                }
                if (type == typeof (string))
                {
                    return true;
                }
                if (type == typeof (double))
                {
                    double d = 0;
                    return double.TryParse(val, out d);
                }
                if (type == typeof (long))
                {
                    long l = 0;
                    return long.TryParse(val, out l);
                }
                if (type == typeof (float))
                {
                    float f = 0;
                    return float.TryParse(val, out f);
                }
                if (type == typeof (bool))
                {
                    bool b;
                    return bool.TryParse(val, out b);
                }
                if (type == typeof (DateTime))
                {
                    var d = DateTime.MinValue;
                    return DateTime.TryParse(val, out d);
                }
                if (type.BaseType == typeof (Enum))
                {
                    Enum.Parse(type, val, true);
                }
            }
            catch (Exception)
            {
                return false;
            }

            //Conversion worked.
            return true;
        }

        /// <summary>
        ///   Validate to see if can convert to appropriate type.
        /// </summary>
        /// <param name="propInfo"> Property to check. </param>
        /// <param name="val"> Instance of object with property. </param>
        /// <returns> True if the conversion can be performed. </returns>
        public static bool CanConvertToCorrectType(PropertyInfo propInfo, object val)
        {
            // Data could be passed as string value.
            // Try to change type to check type safety.                    
            try
            {
                if (propInfo.PropertyType == typeof (int))
                {
                    int i = Convert.ToInt32(val);
                }
                else if (propInfo.PropertyType == typeof (double))
                {
                    double d = Convert.ToDouble(val);
                }
                else if (propInfo.PropertyType == typeof (long))
                {
                    double l = Convert.ToInt64(val);
                }
                else if (propInfo.PropertyType == typeof (float))
                {
                    double f = Convert.ToSingle(val);
                }
                else if (propInfo.PropertyType == typeof (bool))
                {
                    bool b = Convert.ToBoolean(val);
                }
                else if (propInfo.PropertyType == typeof (DateTime))
                {
                    DateTime d = Convert.ToDateTime(val);
                }
                else if (propInfo.PropertyType.BaseType == typeof (Enum) && val is string)
                {
                    Enum.Parse(propInfo.PropertyType, (string) val, true);
                }
            }
            catch (Exception)
            {
                return false;
            }

            //Conversion worked.
            return true;
        }

        /// <summary>
        ///   Checks whether or not the supplied string can be converted to the type designated by the supplied property.
        /// </summary>
        /// <param name="propInfo"> The property representing the type to convert val to </param>
        /// <param name="val"> The value to test for conversion to the type associated with the property </param>
        /// <returns> True if the conversion can be performed. </returns>
        public static bool CanConvertToCorrectType(PropertyInfo propInfo, string val)
        {
            // Data could be passed as string value.
            // Try to change type to check type safety.                    
            try
            {
                if (propInfo.PropertyType == typeof (int))
                {
                    int result;
                    return int.TryParse(val, out result);
                }
                if (propInfo.PropertyType == typeof (string))
                {
                    return true;
                }
                if (propInfo.PropertyType == typeof (double))
                {
                    double d;
                    return double.TryParse(val, out d);
                }
                if (propInfo.PropertyType == typeof (long))
                {
                    long l;
                    return long.TryParse(val, out l);
                }
                if (propInfo.PropertyType == typeof (float))
                {
                    float f;
                    return float.TryParse(val, out f);
                }
                if (propInfo.PropertyType == typeof (bool))
                {
                    bool b;
                    return bool.TryParse(val, out b);
                }
                if (propInfo.PropertyType == typeof (DateTime))
                {
                    var d = DateTime.MinValue;
                    return DateTime.TryParse(val, out d);
                }
                if (propInfo.PropertyType.BaseType == typeof (Enum))
                {
                    Enum.Parse(propInfo.PropertyType, val, true);
                }
            }
            catch (Exception)
            {
                return false;
            }

            //Conversion worked.
            return true;
        }

        /// <summary>
        ///   Convert the val from string type to the same time as the property.
        /// </summary>
        /// <param name="propInfo"> Property representing the type to convert to </param>
        /// <param name="val"> val to convert </param>
        /// <returns> converted value with the same time as the property </returns>
        public static object ConvertToSameType(PropertyInfo propInfo, object val)
        {
            object convertedType = null;

            if (propInfo.PropertyType == typeof (int))
            {
                convertedType = Convert.ChangeType(val, typeof (int));
            }
            else if (propInfo.PropertyType == typeof (double))
            {
                convertedType = Convert.ChangeType(val, typeof (double));
            }
            else if (propInfo.PropertyType == typeof (long))
            {
                convertedType = Convert.ChangeType(val, typeof (long));
            }
            else if (propInfo.PropertyType == typeof (float))
            {
                convertedType = Convert.ChangeType(val, typeof (float));
            }
            else if (propInfo.PropertyType == typeof (bool))
            {
                convertedType = Convert.ChangeType(val, typeof (bool));
            }
            else if (propInfo.PropertyType == typeof (DateTime))
            {
                convertedType = Convert.ChangeType(val, typeof (DateTime));
            }
            else if (propInfo.PropertyType == typeof (string))
            {
                convertedType = Convert.ChangeType(val, typeof (string));
            }
            else if (propInfo.PropertyType.BaseType == typeof (Enum) && val is string)
            {
                convertedType = Enum.Parse(propInfo.PropertyType, (string) val, true);
            }
            return convertedType;
        }

        /// <summary>
        ///   Determine if the type of the property and the val are the same
        /// </summary>
        /// <param name="propInfo"> Property whose type is to be compared. </param>
        /// <param name="val"> Object whose type is to be compared. </param>
        /// <returns> True if the property and the object are of the same type. </returns>
        public static bool IsSameType(PropertyInfo propInfo, object val)
        {
            // Quick Validation.
            if (propInfo.PropertyType == typeof (int) && val is int)
            {
                return true;
            }
            if (propInfo.PropertyType == typeof (bool) && val is bool)
            {
                return true;
            }
            if (propInfo.PropertyType == typeof (string) && val is string)
            {
                return true;
            }
            if (propInfo.PropertyType == typeof (double) && val is double)
            {
                return true;
            }
            if (propInfo.PropertyType == typeof (long) && val is long)
            {
                return true;
            }
            if (propInfo.PropertyType == typeof (float) && val is float)
            {
                return true;
            }
            if (propInfo.PropertyType == typeof (DateTime) && val is DateTime)
            {
                return true;
            }
            return propInfo.PropertyType == val.GetType();
        }
    }
}
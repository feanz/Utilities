using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class TypeHelper
    {
		public static object CovertToType(Type type, object value)
		{
			object convertedType = null;

			if (type == null || value == null)
				return null;

			if (type.IsEnum)
			{
				if (Enum.IsDefined(type, value))
					convertedType = Enum.Parse(type, (string)value);
			}
			else if (type == typeof(bool)) //handle a better range of bool type values 
			{
				var str = value.ToString();
				convertedType = (str == "1" || str == "true" || str == "on" || str == "checked");
			}
			else if (type == typeof(Uri))
				convertedType = new Uri(Convert.ToString(value));
			else
				convertedType = Convert.ChangeType(value, type);

			return convertedType;
		}

	    public static bool IsNumeric(Type type)
	    {
		    return NumericTypes().ContainsKey(type.Name);
	    }

	    public static string GetName<T>(T item) where T : class
		{
			var properties = typeof(T).GetProperties();
			if (properties.Length != 1)
				throw new ArgumentException("Can't get name when item has more than one property", "properties");
			return properties[0].Name;
		}

        public static IDictionary<string, bool> BasicTypes()
        {
            IDictionary<string, bool> basicTypes = new Dictionary<string, bool>();

            basicTypes[typeof (int).Name] = true;
            basicTypes[typeof (long).Name] = true;
            basicTypes[typeof (float).Name] = true;
            basicTypes[typeof (double).Name] = true;
            basicTypes[typeof (decimal).Name] = true;
            basicTypes[typeof (sbyte).Name] = true;
            basicTypes[typeof (Int16).Name] = true;
            basicTypes[typeof (Int32).Name] = true;
            basicTypes[typeof (Int64).Name] = true;
            basicTypes[typeof (Double).Name] = true;
            basicTypes[typeof (Decimal).Name] = true;
            basicTypes[typeof (bool).Name] = true;
            basicTypes[typeof (DateTime).Name] = true;
            basicTypes[typeof (string).Name] = true;

            return basicTypes;
        }

        public static IDictionary<string, bool> NumericTypes()
        {
            IDictionary<string, bool> numericTypes = new Dictionary<string, bool>();

            numericTypes[typeof (int).Name] = true;
            numericTypes[typeof (long).Name] = true;
            numericTypes[typeof (float).Name] = true;
            numericTypes[typeof (double).Name] = true;
            numericTypes[typeof (decimal).Name] = true;
            numericTypes[typeof (sbyte).Name] = true;
            numericTypes[typeof (Int16).Name] = true;
            numericTypes[typeof (Int32).Name] = true;
            numericTypes[typeof (Int64).Name] = true;
            numericTypes[typeof (Double).Name] = true;
            numericTypes[typeof (Decimal).Name] = true;

            return numericTypes;
        }
    }
}
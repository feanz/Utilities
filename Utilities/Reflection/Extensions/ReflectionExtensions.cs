using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Utilities.Reflection.Extensions
{
	public static class ReflectionExtensions
	{
		/// <summary>
		///   Gets the specified first instance of the attributeToFind from the source property
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="property"> The property info who's attributeToFind we are fetching. </param>
		/// <param name="inherit"> Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <returns> The attributes of supplied property info. </returns>
		public static T GetAttribute<T>(this PropertyInfo property, bool inherit = true) where T : Attribute
		{
			if (property != null)
			{
				var attributes = property.GetCustomAttributes(typeof(T), inherit);
				if (attributes.Any())
				{
					return (T)attributes.First();
				}
			}
			return null;
		}

		/// <summary>
		///   Get the DisplayName of the provided property info item if it has one else return the property name
		/// </summary>
		/// <param name="type"> Type of items to use. </param>
		/// <returns> The DisplayName attributeToFind of type or TyneName if no display name. </returns>
		public static string GetDisplayName(this Type type)
		{
			var name = type.Name;
			var displayName = type.GetCustomAttributes(typeof(DisplayNameAttribute), true);
			if (displayName.Any())
				name = ((DisplayNameAttribute)displayName.First()).DisplayName;

			return name;
		}

		/// <summary>
		///   Get the DisplayName of the provided property info item if it has one else return the property name
		/// </summary>
		/// <returns> The DisplayName attributeToFind of property or property name if no display name. </returns>
		public static string GetDisplayName(this PropertyInfo pi)
		{
			var name = pi.Name;
			var displayName = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
			if (displayName.Any())
				name = ((DisplayNameAttribute)displayName.First()).DisplayName;

			return name;
		}

		/// <summary>
		///   Get the properties of a type which have an attributeToFind
		/// </summary>
		/// <param name="t"> A class type. </param>
		/// <returns> List of properties </returns>
		public static List<PropertyInfo> GetPropertyWithAttribute<T>(this Type t) where T : Attribute
		{
			return t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.GetCustomAttributes(typeof(T), false).Count() == 1).ToList();
		}

		/// <summary>
		///   Gets the specified property value from the source object
		/// </summary>
		/// <param name="instance"> Object which we want to get property value for. </param>
		/// <param name="propertyName"> The name of the property we want to get. </param>
		/// <returns> </returns>
		public static object GetPropertyValue(this object instance, string propertyName)
		{
			if (instance != null)
			{
				var property = instance
				   .GetType()
				   .GetProperty(propertyName);

				if (property != null)
					return property.GetValue(instance, null);
			}
			return null;
		}

		/// <summary>
		///   Gets the specified property value from the source object as the specified type
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="instance"> Object which we want to get property value for. </param>
		/// <param name="propertyName"> The name of the property we want to get. </param>
		/// <returns> </returns>
		public static T GetPropertyValue<T>(this object instance, string propertyName)
		{
			if (instance != null)
			{
				var property = instance
					.GetType()
					.GetProperty(propertyName);

				if (property != null)
					return (T)property.GetValue(instance, null);
			}
			return default(T);
		}

		/// <summary>
		///   Verifies that the source property has the specified generic attributeToFind
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="property"> The property to check. </param>
		/// <param name="inhert"> Specifies whether to search this member's inheritance chain to find the attributes. </param>
		/// <returns> </returns>
		public static bool HasAttribute<T>(this PropertyInfo property, bool inhert = true) where T : Attribute
		{
			return (property.GetCustomAttributes(typeof(T), true).Length > 0);
		}
		
		/// <summary>
		///   Determines whether objects of the supplied type are numeric.
		/// </summary>
		/// <param name="type"> Type to check. </param>
		/// <returns> True if objects of the supplied type are numeric. </returns>
		public static bool IsNumeric(this Type type)
		{
			return TypeHelper.NumericTypes().ContainsKey(type.Name);
		}

		/// <summary>
		///   Sets the specified property to the specified value on the source object
		/// </summary>
		/// <param name="instance"> Object whose property will be set. </param>
		/// <param name="propertyName"> Property name to set. </param>
		/// <param name="value"> Property value to set. </param>
		/// <returns> </returns>
		public static object SetProperty(this object instance, string propertyName, object value)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("propertyName");

			var property = instance
				.GetType()
				.GetProperty(propertyName);

			if (property == null)
				throw new InvalidOperationException(string.Format("Object provided does not contain a property named {0}", propertyName));

			if (!property.CanWrite)
				throw new InvalidOperationException(propertyName + " can't have its value set check that it is a settable property");

			property.SetValue(instance, value, null);

			return instance;
		}

		/// <summary>
		///   Sets the specified property to supplied value on the source object (Will make best attempt to convert type to property type)
		/// </summary>
		/// <param name="instance"> Object whose property will be set. </param>
		/// <param name="propertyName"> Property name to set. </param>
		/// <param name="value"> Property value to set. </param>
		/// <returns> </returns>
		public static object SetProperty(this object instance, string propertyName, string value)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("propertyName");

			var type = instance.GetType();
			var property = type.GetProperty(propertyName);

			
			if (property == null)
				throw new InvalidOperationException(string.Format("Object provided does not contain a property named {0}", propertyName));

			if (!property.CanWrite)
				throw new InvalidOperationException(propertyName + " can't have its value set check that it is a settable property");

			var convertedVal = TypeHelper.CovertToType(property.PropertyType, value);
			property.SetValue(instance, convertedVal, null);

			return instance;
		}

		/// <summary>
		///   Set the object properties using the prop name and value.
		/// </summary>
		/// <typeparam name="T"> Type of property to set. </typeparam>
		/// <param name="instance"> Object whose property will be set. </param>
		/// <param name="propertyName"> Property name to set. </param>
		/// <param name="value"> Property value to set. </param>
		public static object SetProperty<T>(this object instance, string propertyName, object value)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException("propertyName");

			var type = instance.GetType();
			var property = type.GetProperty(propertyName);


			if (property == null)
				throw new InvalidOperationException(string.Format("Object provided does not contain a property named {0}", propertyName));

			if (!property.CanWrite)
				throw new InvalidOperationException(propertyName + " can't have its value set check that it is a settable property");

			var convertedVal = (T) value;
			property.SetValue(instance, convertedVal, null);

			return instance;
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Utilities.Extensions
{
	public static class CollectionExtensions
	{
		/// <summary>
		///     AddRange of items of same type to IList.
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="items"> The list to be added to. </param>
		/// <param name="itemsToAdd"> The items to be added. </param>
		/// <returns> The new combined list. </returns>
		public static IList<T> AddRange<T>(this IList<T> items, IList<T> itemsToAdd)
		{
			if (items == null || itemsToAdd == null)
				return items;

			foreach (var item in itemsToAdd)
				items.Add(item);

			return items;
		}

		/// <summary>
		///     Validate for any nulls.
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="items"> List of items. </param>
		/// <returns> True if a null is present in the items. </returns>
		public static bool AnyNulls<T>(this IEnumerable<T> items) where T : class
		{
			return items.Any(item => item == null);
		}

		/// <summary>
		///     Append the specified bit array to the specific bit array.
		/// </summary>
		/// <param name="current"> The array to be appended to. </param>
		/// <param name="after"> The array to be appended on. </param>
		/// <returns> The new combined bit array. </returns>
		public static BitArray Append(this BitArray current, BitArray after)
		{
			var bools = new bool[current.Count + after.Count];
			current.CopyTo(bools, 0);
			after.CopyTo(bools, current.Count);
			return new BitArray(bools);
		}

		/// <summary>
		///     Converts a generic List collection to a single string using the specified delimitter.
		/// </summary>
		/// <param name="items"> The items. </param>
		/// <param name="delimiter"> The delimiter. </param>
		/// <returns> The new delimited string. </returns>
		public static string AsDelimited<T>(this IEnumerable<T> items, string delimiter)
		{
			return items == null ? string.Empty : string.Join(delimiter, items);
		}

		/// <summary>
		///     Enumerates the specified items of items and applies the specified action
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="collection"> The items on which to iterate over. </param>
		/// <param name="action"> The action to be applied to each item </param>
		/// <returns> The list of items just iterated over. </returns>
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection)
			{
				action(item);
			}
			return collection;
		}

		/// <summary>
		///     Determines whether the specified enumerable collection is empty. Note: This method has the side effect of moving
		///     the position of the enumerator back to the starting position.
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="items"> Enumerable to test </param>
		/// <returns> <c>true</c> if the specified collection is null or empty; otherwise, <c>false</c> . </returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
		{
			return items == null || !items.Any();
		}

		/// <summary>
		///     Validate if all of the items in the collection satisfied by the condition.
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="items"> List of items. </param>
		/// <param name="executor"> Function to call for each item. </param>
		/// <returns> True if the executor returned true for all items. </returns>
		public static bool IsTrueForAll<T>(this IEnumerable<T> items, Func<T, bool> executor)
		{
			return items.Select(executor).All(result => result);
		}

		/// <summary>
		///     Validate if any of the items in the collection satisfied by the condition.
		/// </summary>
		/// <typeparam name="T"> Type of items to use. </typeparam>
		/// <param name="items"> List of items to use. </param>
		/// <param name="executor"> Function to call for each item. </param>
		/// <returns> True if the executor returned True for at least one item. </returns>
		public static bool IsTrueForAny<T>(this IEnumerable<T> items, Func<T, bool> executor)
		{
			return items.Select(executor).Any(result => result);
		}

		/// <summary>
		///     Creates a text items from the specified items.
		/// </summary>
		/// <param name="items"> The items to add to the text. </param>
		/// <param name="newLine"> The new line char to be used. Defaults to '\r\n'. </param>
		/// <returns> The new text list. </returns>
		public static string TextList(this IEnumerable<string> items, string newLine = "\r\n")
		{
			return string.Join(newLine, items.ToArray());
		}

		/// <summary>
		///     Change items to comma separated string.
		/// </summary>
		/// <param name="items"> The items to be converted to comma delimited list. </param>
		/// <returns> New comma delimited string of the items provided. </returns>
		public static string ToCommaSeparatedString(this IEnumerable<string> items)
		{
			return AsDelimited(items, ",");
		}

		/// <summary>
		///     Convert IEnumerable to a datatable
		/// </summary>
		/// <param name="items"> The set of items to be converted to a datatable. </param>
		/// <returns> </returns>
		public static DataTable ToDataTable(this IEnumerable items)
		{
			var dtReturn = new DataTable();
			PropertyInfo[] oProps = null;

			foreach (var rec in items)
			{
				if (dtReturn.Columns.Count == 0)
				{
					// Use reflection to get property names, to create table
					// column names
					var type = rec.GetType();
					dtReturn.TableName = type.Name;
					oProps = type.GetProperties();

					foreach (var pi in oProps)
					{
						if (!pi.PropertyType.IsNullable())
							dtReturn.Columns.Add(pi.Name, pi.PropertyType);
						else
						{
							var converter = new NullableConverter(pi.PropertyType);
							dtReturn.Columns.Add(pi.Name, converter.UnderlyingType);
						}
					}
				}

				var dr = dtReturn.NewRow();
				if (oProps != null)
					foreach (var pi in oProps)
					{
						if (!pi.PropertyType.IsNullable())
							dr[pi.Name] = pi.GetValue(rec, null);
						else
						{
							object obj = pi.GetValue(rec, null);
							if (obj != null)
							{
								var converter = new NullableConverter(pi.PropertyType);
								dr[pi.Name] = Convert.ChangeType(obj, converter.UnderlyingType);
							}
						}
					}
				dtReturn.Rows.Add(dr);
			}

			return (dtReturn);
		}


		/// <summary>
		///     Converts string[] array to arry of integers.
		/// </summary>
		/// <param name="source"> The array of strings to convert to integers </param>
		/// <returns> The new integer array. </returns>
		public static int[] ToIntArray(this string[] source)
		{
			return source.Select(x => int.Parse(x, CultureInfo.CurrentCulture)).ToArray();
		}
	}
}
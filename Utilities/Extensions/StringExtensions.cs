using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.Extensions
{
	public static class StringExtensions
	{
		#region Boolean

		/// <summary>
		///     Validate that string contains string case insensitive search
		/// </summary>
		/// <param name="text"> </param>
		/// <param name="contains"> </param>
		/// <returns> </returns>
		public static bool ContainsCaseInsensitive(this string text, string contains)
		{
			return text.IndexOf(contains, StringComparison.OrdinalIgnoreCase) != -1;
		}

		/// <summary>
		///     Verifies that the source text is not null or empty
		/// </summary>
		/// <param name="text"> </param>
		/// <returns> </returns>
		public static bool IsNotNullOrEmpty(this string text)
		{
			return !text.IsNullOrEmpty();
		}

		/// <summary>
		///     Verifies that the source text is null or empty
		/// </summary>
		/// <param name="text"> </param>
		/// <returns> </returns>
		public static bool IsNullOrEmpty(this string text)
		{
			return string.IsNullOrEmpty(text);
		}

		/// <summary>
		///     Verifies that the source text matches the specified match text using the provided comparison
		/// </summary>
		/// <param name="text"> </param>
		/// <param name="match"> </param>
		/// <param name="comparison"></param>
		/// <returns> </returns>
		public static bool IsSameAs(this string text, string match, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return text != null && text.Equals(match, comparison);
		}

		/// <summary>
		///     Verifies that the source text does not equal the specified value using the specified string comparison
		/// </summary>
		/// <param name="text"> </param>
		/// <param name="match"> </param>
		/// <param name="comparison"> </param>
		/// <returns> </returns>
		public static bool NotSameAs(this string text, string match,
			StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
		{
			return !text.IsSameAs(match, comparison);
		}

		/// <summary>
		///     Verifies that the source text matches the specified match text using StringComparison.InvariantCultureIgnoreCase
		/// </summary>
		/// <param name="text"> </param>
		/// <param name="match"> </param>
		/// <returns> </returns>
		public static bool ToICIC(this string text, string match)
		{
			return text.IsNotNull() && text.Equals(match, StringComparison.InvariantCultureIgnoreCase);
		}

		#endregion

		#region Strings

		/// <summary>
		///     Converts ASCII encoding to Unicode
		/// </summary>
		/// <param name="asciiCode"> The ASCII code. </param>
		/// <returns> </returns>
		public static string AsciiToUnicode(this int asciiCode)
		{
			var ascii = Encoding.UTF32;
			var c = (char) asciiCode;
			var b = ascii.GetBytes(c.ToString(CultureInfo.InvariantCulture));
			return ascii.GetString((b));
		}

		/// <summary>
		///     Strips the last specified chars from a string.
		/// </summary>
		/// <param name="text"> The source string. </param>
		/// <param name="removeFromEnd"> The remove from end. </param>
		/// <returns> </returns>
		public static string Chop(this string text, int removeFromEnd)
		{
			string result = text;
			if ((removeFromEnd > 0) && (text.Length > removeFromEnd - 1))
				result = result.Remove(text.Length - removeFromEnd, removeFromEnd);
			return result;
		}

		/// <summary>
		///     Strips the last specified chars from a string.
		/// </summary>
		/// <param name="text"> The source string. </param>
		/// <param name="backDownTo"> The back down to. </param>
		/// <returns> </returns>
		public static string Chop(this string text, string backDownTo)
		{
			var removeDownTo = text.LastIndexOf(backDownTo, StringComparison.CurrentCultureIgnoreCase);
			var removeFromEnd = 0;
			if (removeDownTo > 0)
				removeFromEnd = text.Length - removeDownTo;

			var result = text;

			if (text.Length > removeFromEnd - 1)
				result = result.Remove(removeDownTo, removeFromEnd);

			return result;
		}

		/// <summary>
		///     Strips the last char from a a string.
		/// </summary>
		/// <param name="text"> The source string. </param>
		/// <returns> </returns>
		public static string Chop(this string text)
		{
			return Chop(text, 1);
		}

		/// <summary>
		///     Removes the specified chars from the beginning of a string.
		/// </summary>
		/// <param name="text"> The source string. </param>
		/// <param name="removeFromBeginning"> The remove from beginning. </param>
		/// <returns> </returns>
		public static string Clip(this string text, int removeFromBeginning)
		{
			string result = text;
			if (text.Length > removeFromBeginning)
				result = result.Remove(0, removeFromBeginning);
			return result;
		}

		/// <summary>
		///     Removes chars from the beginning of a string, up to the specified string
		/// </summary>
		/// <param name="text"> The source string. </param>
		/// <param name="removeUpTo"> The remove up to. </param>
		/// <returns> </returns>
		public static string Clip(this string text, string removeUpTo)
		{
			int removeFromBeginning = text.IndexOf(removeUpTo, StringComparison.CurrentCultureIgnoreCase);
			string result = text;

			if (text.Length > removeFromBeginning && removeFromBeginning > 0)
				result = result.Remove(0, removeFromBeginning);

			return result;
		}

		/// <summary>
		///     Strips the last char from a a string.
		/// </summary>
		/// <param name="text"> The source string. </param>
		/// <returns> </returns>
		public static string Clip(this string text)
		{
			return Clip(text, 1);
		}
		
		/// <summary>
		///     Formats the source text using the specified parameters
		/// </summary>
		/// <param name="text"> </param>
		/// <param name="arguments"> </param>
		/// <returns> </returns>
		public static string Format(this string text, params object[] arguments)
		{
			return string.Format(text, arguments);
		}

		/// <summary>
		///     Returns the start of the string up to the specified max length
		/// </summary>
		/// <param name="text"> </param>
		/// <param name="maxLength"> </param>
		/// <returns> </returns>
		public static string MaxLength(this string text, int maxLength)
		{
			return (text.Length > maxLength ? text.Substring(0, maxLength - 1) : text);
		}

		/// <summary>
		///     Removes any tab spaces from the source text
		/// </summary>
		/// <param name="text"> </param>
		/// <returns> </returns>
		public static string RemoveTabs(this string text)
		{
			if (text.IsNotNullOrEmpty())
			{
				var match = Regex.Match(text, @"[^\t]+");
				if (match.Success)
				{
					return match.Value;
				}
			}
			return null;
		}

		/// <summary>
		///     Removes all line feeds, carriage returns and tab spaces from the source text
		/// </summary>
		/// <param name="text"> </param>
		/// <returns> </returns>
		public static string SingleLine(this string text)
		{
			return text.Replace("\r\n", string.Empty).Replace("\t", string.Empty).Replace("> <", "><");
		}

		/// <summary>
		///     Strips all HTML tags from a string
		/// </summary>
		/// <param name="htmlValue"> The HTML string. </param>
		/// <returns> </returns>
		public static string StripHTML(this string htmlValue)
		{
			return StripHTML(htmlValue, String.Empty);
		}

		/// <summary>
		///     Strips all HTML tags from a string and replaces the tags with the specified replacement
		/// </summary>
		/// <param name="htmlValue"> The HTML string. </param>
		/// <param name="htmlPlaceHolder"> The HTML place holder. </param>
		/// <returns> </returns>
		public static string StripHTML(this string htmlValue, string htmlPlaceHolder)
		{
			const string pattern = @"<(.|\n)*?>";
			string sOut = Regex.Replace(htmlValue, pattern, htmlPlaceHolder);
			sOut = sOut.Replace("&nbsp;", String.Empty);
			sOut = sOut.Replace("&amp;", "&");
			sOut = sOut.Replace("&gt;", ">");
			sOut = sOut.Replace("&lt;", "<");
			return sOut;
		}

		/// <summary>
		///     Replaces the tabs in the source text with the specified number of spaces
		/// </summary>
		/// <param name="text"> </param>
		/// <param name="spacesPerTab"> </param>
		/// <returns> </returns>
		public static string TabsToSpaces(this string text, int spacesPerTab = 16)
		{
			if (text.IsNotNullOrEmpty())
			{
				return text.Replace("\t", new string(' ', spacesPerTab));
			}
			return text;
		}

		/// <summary>
		///     Formats the source text as title case (i.e. "ChristopherJamesFairbairn" would become "Christopher James Fairbairn")
		/// </summary>
		/// <param name="text"> </param>
		/// <returns> </returns>
		public static string TitleCase(this string text)
		{
			if (text.IsNotNullOrEmpty())
			{
				var output = string.Empty;
				text.ToCharArray().ForEach(c => output += (char.IsUpper(c) ? " " : string.Empty) + c);
				return output.Trim();
			}
			return text;
		}

		/// <summary>
		///     Get a users username from a domain name string
		/// </summary>
		/// <param name="source"> </param>
		/// <returns> </returns>
		public static string ToUserName(this string source)
		{
			if (!source.Contains(@"\"))
				throw new ArgumentException("String is not formated as a domain user");

			var split = source.Split('\\');

			return split[1];
		}

		#endregion

		#region Collections

		/// <summary>
		///     Split the specified text into lines
		/// </summary>
		/// <param name="text"> </param>
		/// <param name="delimiter"> </param>
		/// <returns> </returns>
		public static string[] SplitLines(this string text, string delimiter = "\r\n")
		{
			return text.IsNotNullOrEmpty() ? text.Split(new[] {delimiter}, StringSplitOptions.None) : null;
		}

		/// <summary>
		///     Converts comma delimited string to array
		/// </summary>
		/// <param name="source"> </param>
		/// <returns> </returns>
		public static string[] ToArray(this string source)
		{
			if (source.IsNotNull())
			{
				return source.ToArray(",");
			}
			throw new Exception("Can't convert null value to array.");
		}

		/// <summary>
		///     Converts delimited string to array by provided delimiter
		/// </summary>
		/// <param name="source"> </param>
		/// <param name="delimiter"> </param>
		/// <returns> If no delimter is found it will return an array with one string </returns>
		public static string[] ToArray(this string source, string delimiter)
		{
			var spliter = new[] {delimiter};

			return !source.Contains(delimiter)
				? new[] {source}
				: source.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		///     Converts comma delimited items of strings
		/// </summary>
		/// <param name="source"> </param>
		/// <returns> </returns>
		public static List<string> ToList(this string source)
		{
			return source.ToList(",");
		}

		/// <summary>
		///     Converts delimited string to items of strings by provided delimiter
		/// </summary>
		/// <param name="source"> </param>
		/// <param name="delimiter"> </param>
		/// <returns> </returns>
		public static List<string> ToList(this string source, string delimiter)
		{
			if (source.IsNotNull())
			{
				var spliter = new[] {delimiter};

				if (!source.Contains(delimiter))
				{
					var result = new List<string> {source};
					return result;
				}

				return source.Split(spliter, StringSplitOptions.RemoveEmptyEntries).ToList();
			}
			return new List<string>();
		}

		#endregion
	}
}
using System.Text.RegularExpressions;

namespace Utilities.Validation.ExtensionMethods
{
	public static class StringExtensions
	{
		/// <summary>
		///   Determines whether the specified  string contains only alpha characters.
		/// </summary>
		/// <param name="alpha"> The  string. </param>
		/// <returns> <c>true</c> if the specified  string is alpha; otherwise, <c>false</c> . </returns>
		public static bool IsAlpha(this string alpha)
		{
			return !Regex.IsMatch(alpha, RegexPattern.ALPHA);
		}

		/// <summary>
		///   Determines whether the specified  string contains only alphanumeric characters
		/// </summary>
		/// <param name="alphaNumeric"> The  string. </param>
		/// <returns> <c>true</c> if the string is alphanumeric; otherwise, <c>false</c> . </returns>
		public static bool IsAlphaNumeric(this string alphaNumeric)
		{
			return !Regex.IsMatch(alphaNumeric, RegexPattern.ALPHA_NUMERIC);
		}

		/// <summary>
		///   Determines whether the specified  string contains only alphanumeric characters
		/// </summary>
		/// <param name="alphaNumeric"> The  string. </param>
		/// <param name="allowSpaces"> if set to <c>true</c> [allow spaces]. </param>
		/// <returns> <c>true</c> if the string is alphanumeric; otherwise, <c>false</c> . </returns>
		public static bool IsAlphaNumeric(this string alphaNumeric, bool allowSpaces)
		{
			if (allowSpaces)
				return !Regex.IsMatch(alphaNumeric, RegexPattern.ALPHA_NUMERIC_SPACE);
			return IsAlphaNumeric(alphaNumeric);
		}

		/// <summary>
		///   Determines whether the specified email address string is valid based on regular expression uation.
		/// </summary>
		/// <param name="emailaddress"> The email address string. </param>
		/// <returns> <c>true</c> if the specified email address is valid; otherwise, <c>false</c> . </returns>
		public static bool IsEmail(this string emailaddress)
		{
			return Regex.IsMatch(emailaddress, RegexPattern.EMAIL);
		}

		/// <summary>
		///   Determines whether the specified string is a valid GUID.
		/// </summary>
		/// <param name="guid"> The GUID. </param>
		/// <returns> <c>true</c> if the specified string is a valid GUID; otherwise, <c>false</c> . </returns>
		public static bool IsGuid(this string guid)
		{
			return Regex.IsMatch(guid, RegexPattern.GUID);
		}

		/// <summary>
		///   Determines whether the specified string is a valid IP address.
		/// </summary>
		/// <param name="ipAddress"> The ip address. </param>
		/// <returns> <c>true</c> if valid; otherwise, <c>false</c> . </returns>
		public static bool IsIPAddress(this string ipAddress)
		{
			return Regex.IsMatch(ipAddress, RegexPattern.IP_ADDRESS);
		}

		/// <summary>
		///   Determines whether the specified string is lower case.
		/// </summary>
		/// <param name="text"> The input string. </param>
		/// <returns> <c>true</c> if the specified string is lower case; otherwise, <c>false</c> . </returns>
		public static bool IsLowerCase(this string text)
		{
			return Regex.IsMatch(text, RegexPattern.LOWER_CASE);
		}

		/// <summary>
		///   Determines whether the specified string is a mobilenumber.
		/// </summary>
		/// <param name="mobileNumber"> the mobilenumber </param>
		/// <returns> <c>true</c> if mobilenumber otherwise, <c>false</c> . </returns>
		public static bool IsMobileNumber(this string mobileNumber)
		{
			return Regex.IsMatch(mobileNumber, RegexPattern.MOBILENUMBER);
		}

		/// <summary>
		///   Determines whether the specified  string contains only numeric characters
		/// </summary>
		/// <param name="numeric"> The  string. </param>
		/// <returns> <c>true</c> if the string is numeric; otherwise, <c>false</c> . </returns>
		public static bool IsNumeric(this string numeric)
		{
			return !Regex.IsMatch(numeric, RegexPattern.NUMERIC);
		}

		/// <summary>
		///   Determines whether the specified string is a postcode.
		/// </summary>
		/// <param name="postcode"> the postcode </param>
		/// <returns> <c>true</c> if postcode; otherwise, <c>false</c> . </returns>
		public static bool IsPostcode(this string postcode)
		{
			return Regex.IsMatch(postcode, RegexPattern.POSTCODE);
		}

		/// <summary>
		///   Determines whether the specified string is consider a strong password based on the supplied string.
		/// </summary>
		/// <param name="password"> The password. </param>
		/// <returns> <c>true</c> if strong; otherwise, <c>false</c> . </returns>
		public static bool IsStrongPassword(this string password)
		{
			return Regex.IsMatch(password, RegexPattern.STRONG_PASSWORD);
		}

		/// <summary>
		///   Determines whether the specified string is a valid URL string using the referenced regex string.
		/// </summary>
		/// <param name="url"> The URL string. </param>
		/// <returns> <c>true</c> if valid; otherwise, <c>false</c> . </returns>
		public static bool IsURL(this string url)
		{
			return Regex.IsMatch(url, RegexPattern.URL);
		}

		/// <summary>
		///   Determines whether the specified string is upper case.
		/// </summary>
		/// <param name="text"> The input string. </param>
		/// <returns> <c>true</c> if the specified string is upper case; otherwise, <c>false</c> . </returns>
		public static bool IsUpperCase(this string text)
		{
			return Regex.IsMatch(text, RegexPattern.UPPER_CASE);
		}


	}
}
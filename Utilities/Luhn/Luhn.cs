using System;
using System.Globalization;
using System.Linq;

namespace Utilities.Luhn
{
	public class Luhn
	{
		/// <summary>
		///     Validate that string is a luhn valid account number
		/// </summary>
		/// <param name="pan"> </param>
		/// <param name="context"> </param>
		/// <returns> </returns>
		public static string ValidateLuhn(string pan, string context = "account number")
		{
			var checkDigit = int.Parse(pan.Substring((pan.Length - 1), 1));
			//Get each number in pan in reverse order that is not the check digit
			var numbers =
				pan.Substring(0, pan.Length - 1).ToCharArray().Select(
					x => int.Parse(x.ToString(CultureInfo.InvariantCulture))).Reverse().ToArray();
			int total = 0;

			for (int i = 0; i < numbers.Length; i++)
			{
				if ((i + 1) % 2 != 0)
				{
					//Double every other number
					numbers[i] = numbers[i] * 2;
				}
				numbers[i] = numbers[i].ToString(CultureInfo.InvariantCulture).ToCharArray().Sum(x => x - '0');
				total += numbers[i];
			}
			if ((total * 9) % 10 != checkDigit)
			{
				throw new Exception(string.Format("The specified {0} was not a valid luhn number.", context));
			}
			return pan;
		} 
	}
}
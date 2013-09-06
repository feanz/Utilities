using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Cryptography.ExtensionMethods
{
	/// <summary>
	///     Hash based extensions
	/// </summary>
	public static class HashExtensions
	{
		/// <summary>
		///     Computes the hash of a byte array
		/// </summary>
		/// <param name="data"> Byte array to hash </param>
		/// <param name="algorithm"> Hash algorithm to use (defaults to SHA256) </param>
		/// <returns> The hash of the byte array </returns>
		public static byte[] Hash(this byte[] data, HashAlgorithm algorithm = null)
		{
			if (data == null)
				throw new ArgumentException("data");

			algorithm = algorithm ?? new SHA256CryptoServiceProvider();
			using (var Hasher = algorithm)
			{
				var hashedArray = Hasher.ComputeHash(data);
				Hasher.Clear();
				return hashedArray;
			}
		}

		/// <summary>
		///     Computes the hash of a string
		/// </summary>
		/// <param name="data"> string to hash </param>
		/// <param name="algorithm"> Algorithm to use (defaults to SHA256) </param>
		/// <param name="encodingUsing"> Encoding used by the string (defaults to UTF8) </param>
		/// <returns> The hash of the string </returns>
		public static string Hash(this string data, HashAlgorithm algorithm = null, Encoding encodingUsing = null)
		{
			if (string.IsNullOrEmpty(data))
				throw new ArgumentException("data");

			return BitConverter.ToString(data.ToByteArray(encodingUsing).Hash(algorithm)).Replace("-", "");
		}

		private static byte[] ToByteArray(this string input, Encoding encodingUsing = null)
		{
			encodingUsing = encodingUsing ?? new UTF8Encoding();
			return (string.IsNullOrEmpty(input)) ? null : encodingUsing.GetBytes(input);
		}
	}
}
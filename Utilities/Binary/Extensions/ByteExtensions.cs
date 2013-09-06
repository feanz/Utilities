using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.Binary.Extensions
{
	public static class ByteExtensions
	{
		#region CompressionType enum

		public enum CompressionType
		{
			Deflate = 0,
			GZip = 1
		}

		#endregion

		/// <summary>
		///     Appends the specified data to the source byte array
		/// </summary>
		/// <param name="data"> Byte array to be appended </param>
		/// <param name="toAppend"> Byte array to append. </param>
		/// <returns> The new combined byte array </returns>
		public static byte[] Append(this byte[] data, byte[] toAppend)
		{
			if (data != null)
			{
				var buffer = new byte[data.Length + toAppend.Length];
				Array.Copy(data, buffer, data.Length);
				Array.Copy(toAppend, 0, buffer, data.Length, toAppend.Length);
				return buffer;
			}
			return null;
		}

		/// <summary>
		///     Converts base 64 string to a byte array
		/// </summary>
		/// <param name="Input"> Input string </param>
		/// <returns> A byte array equivalent of the base 64 string </returns>
		public static byte[] Base64ToByteArray(this string Input)
		{
			return string.IsNullOrEmpty(Input) ? new byte[0] : Convert.FromBase64String(Input);
		}

		/// <summary>
		///     Convert binary string representation to byte array
		/// </summary>
		/// <param name="binary"> Binary string </param>
		/// <returns> </returns>
		public static byte[] BinaryToByteArray(this string binary)
		{
			if (binary.Length%8 != 0)
			{
				throw new ArgumentException("The input string did not describe a complete byte.");
			}

			var isValidBinary = IsBinary(binary);

			if (!isValidBinary)
			{
				throw new ArgumentException("The input string was not in binary format.");
			}

			var length = (binary.Length/8);
			var bytes = new byte[length];
			for (var i = 0; i < length; i++)
			{
				bytes[i] = Convert.ToByte(binary.Substring(8*i, 8), 2);
			}
			return bytes;
		}

		/// <summary>
		///     Compresses the data using the specified compression type
		/// </summary>
		/// <param name="data"> data to compress </param>
		/// <param name="compressionType"> Compression type </param>
		/// <returns> The compressed data </returns>
		public static byte[] Compress(this byte[] data, CompressionType compressionType = CompressionType.Deflate)
		{
			if (data == null)
				throw new ArgumentException("data");

			using (var stream = new MemoryStream())
			{
				using (var zipStream = GetStream(stream, CompressionMode.Compress, compressionType))
				{
					zipStream.Write(data, 0, data.Length);
					zipStream.Close();
					return stream.ToArray();
				}
			}
		}

		/// <summary>
		///     Compresses a string of data
		/// </summary>
		/// <param name="data"> data to Compress </param>
		/// <param name="encodingUsing"> Encoding that the data uses (defaults to UTF8) </param>
		/// <param name="compressionType"> The compression type used </param>
		/// <returns> The data Compressed </returns>
		public static string Compress(this string data, Encoding encodingUsing = null,
			CompressionType compressionType = CompressionType.Deflate)
		{
			if (data == null)
				throw new ArgumentException("data");

			return data.ToByteArray(encodingUsing).Compress(compressionType).ToBase64String();
		}

		/// <summary>
		///     Decompresses the byte array that is sent in
		/// </summary>
		/// <param name="data"> data to decompress </param>
		/// <param name="CompressionType"> The compression type used </param>
		/// <returns> The data decompressed </returns>
		public static byte[] Decompress(this byte[] data, CompressionType CompressionType = CompressionType.Deflate)
		{
			if (data == null)
				throw new ArgumentException("data");

			using (var stream = new MemoryStream())
			{
				using (var zipStream = GetStream(new MemoryStream(data), CompressionMode.Decompress, CompressionType))
				{
					var buffer = new byte[4096];
					while (true)
					{
						int size = zipStream.Read(buffer, 0, buffer.Length);
						if (size > 0) stream.Write(buffer, 0, size);
						else break;
					}
					zipStream.Close();
					return stream.ToArray();
				}
			}
		}

		/// <summary>
		///     Decompresses a string of data
		/// </summary>
		/// <param name="data"> data to decompress </param>
		/// <param name="EncodingUsing"> Encoding that the result should use (defaults to UTF8) </param>
		/// <param name="CompressionType"> The compression type used </param>
		/// <returns> The data decompressed </returns>
		public static string Decompress(this string data, Encoding EncodingUsing = null,
			CompressionType CompressionType = CompressionType.Deflate)
		{
			if (data == null)
				throw new ArgumentException("data");

			return data.Base64ToByteArray().Decompress(CompressionType).ToEncodedString(EncodingUsing);
		}

		/// <summary>
		///     Converts a hexadecimal string to a byte array representation.
		/// </summary>
		/// <param name="hex"> Hexadecimal string to convert to byte array. </param>
		/// <returns> Byte array representation of the string. </returns>
		/// <remarks>
		///     The string is assumed to be of even size.
		/// </remarks>
		public static byte[] HexToByteArray(this string hex)
		{
			var b = new byte[hex.Length/2];
			for (var i = 0; i < hex.Length; i += 2)
			{
				b[i/2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}
			return b;
		}

		/// <summary>
		///     Validate if string is a valide binary representation
		/// </summary>
		/// <param name="binary"> </param>
		/// <returns> </returns>
		public static bool IsBinary(this string binary)
		{
			var isValidBinary = true;
			var chars = binary.ToCharArray();

			foreach (var t in chars)
			{
				isValidBinary = (t == '0' || t == '1');
			}
			return isValidBinary;
		}

		/// <summary>
		///     Validate is string is a valide hex representation
		/// </summary>
		/// <param name="hex"> </param>
		/// <returns> </returns>
		public static bool IsHex(this string hex)
		{
			var chars = hex.ToCharArray();
			var isHexChar = true;
			for (var i = 0; i < chars.Length; i++)
			{
				isHexChar = (chars[i] >= '0' && chars[i] <= '9') ||
				            (chars[i] >= 'a' && chars[i] <= 'f') ||
				            (chars[i] >= 'A' && chars[i] <= 'F');
			}
			return isHexChar;
		}

		/// <summary>
		///     Determines if a byte array is unicode
		/// </summary>
		/// <param name="data"> Input array </param>
		/// <returns> True if it's unicode, false otherwise </returns>
		public static bool IsUnicode(this byte[] data)
		{
			return data == null || data.ToEncodedString(new UnicodeEncoding()).IsUnicode();
		}

		/// <summary>
		///     Determines if a string is unicode
		/// </summary>
		/// <param name="text"> Input string </param>
		/// <returns> True if it's unicode, false otherwise </returns>
		public static bool IsUnicode(this string text)
		{
			return string.IsNullOrEmpty(text) || Regex.Replace(text, @"[^\u0000-\u007F]", "") != text;
		}

		/// <summary>
		///     Converts a byte array into a base 64 string
		/// </summary>
		/// <param name="data"> Input array </param>
		/// <returns> The equivalent byte array in a base 64 string </returns>
		public static string ToBase64String(this byte[] data)
		{
			return data == null ? "" : Convert.ToBase64String(data);
		}

		/// <summary>
		///     Convert byte array to a binary representation as a string
		/// </summary>
		/// <param name="data"> The byte array to be converted to a string </param>
		/// <returns> A binary string that represents the byte array </returns>
		public static string ToBinary(this byte[] data)
		{
			var bits = new BitArray(data);
			var output = string.Empty;

			var octets = bits.Length/8;
			for (var o = 0; o < octets; o++)
			{
				for (var b = 7; b >= 0; b--)
				{
					var index = (o*8) + b;
					output += (bits[index] ? "1" : "0");
				}
			}

			return output;
		}

		/// <summary>
		///     Converts a string to a byte array
		/// </summary>
		/// <param name="input"> input string </param>
		/// <param name="encodingUsing"> The type of encoding the string is using (defaults to UTF8) </param>
		/// <returns> the byte array representing the string </returns>
		public static byte[] ToByteArray(this string input, Encoding encodingUsing = null)
		{
			encodingUsing = encodingUsing ?? new UTF8Encoding();
			return (string.IsNullOrEmpty(input)) ? null : encodingUsing.GetBytes(input);
		}

		/// <summary>
		///     Converts a byte array to a string
		/// </summary>
		/// <param name="data"> input array </param>
		/// <param name="encodingUsing"> The type of encoding the string is using (defaults to UTF8) </param>
		/// <param name="index"> The index of the first byte to decode </param>
		/// <param name="count">
		///     Number of bytes starting at the index to convert (use -1 for the entire array starting at the
		///     index)
		/// </param>
		/// <returns> string of the byte array </returns>
		public static string ToEncodedString(this byte[] data, Encoding encodingUsing = null, int index = 0,
			int count = -1)
		{
			if (data == null)
				return "";
			if (count == -1)
				count = data.Length - index;
			encodingUsing = encodingUsing ?? new UTF8Encoding();
			return encodingUsing.GetString(data, index, count);
		}


		/// <summary>
		///     Returns the binary representation of a hexadecimal number.
		/// </summary>
		/// <param name="hex"> Binary string to convert to hexadecimal. </param>
		/// <returns> Hexadecimal representation of string. </returns>
		public static string HexToBinary(this string hex)
		{
			return Convert.ToString(Convert.ToInt32(hex, 16), 2);
		}

		/// <summary>
		///     Returns the decimal representation of a hexadecimal number.
		/// </summary>
		/// <param name="hex"> Hexadecimal string to convert to decimal. </param>
		/// <returns> Decimal representation of string. </returns>
		public static string HexToDecimal(this string hex)
		{
			return Convert.ToString(Convert.ToInt32(hex, 16));
		}

		/// <summary>
		///     Returns the binary representation of a binary number.
		/// </summary>
		/// <param name="txt"> Decimal string to convert to binary. </param>
		/// <returns> Binary representation of string. </returns>
		public static string DecimalToBinary(this string txt)
		{
			return Convert.ToString(Convert.ToInt32(txt), 2);
		}

		/// <summary>
		///     Returns the hexadecimal representation of a decimal number.
		/// </summary>
		/// <param name="txt"> Hexadecimal string to convert to decimal. </param>
		/// <returns> Decimal representation of string. </returns>
		public static string DecimalToHex(this string txt)
		{
			return Convert.ToString(Convert.ToInt32(txt), 16);
		}


		/// <summary>
		///     Returns the decimal representation of a binary number.
		/// </summary>
		/// <param name="text"> Binary string to convert to decimal. </param>
		/// <returns> Decimal representation of string. </returns>
		public static string BinaryToDecimal(this string text)
		{
			return Convert.ToString(Convert.ToInt32(text, 2));
		}

		/// <summary>
		///     Returns the hexadecimal representation of a binary number.
		/// </summary>
		/// <param name="text"> Binary string to convert to hexadecimal. </param>
		/// <returns> Hexadecimal representation of string. </returns>
		public static string BinaryToHex(this string text)
		{
			return Convert.ToString(Convert.ToInt32(text, 2), 16);
		}

		/// <summary>
		///     Converts a byte array to a hexadecimal string representation.
		/// </summary>
		/// <param name="data"> Byte array to convert to hexadecimal string. </param>
		/// <returns> String representation of byte array. </returns>
		public static string ToHex(this byte[] data)
		{
			return BitConverter.ToString(data).Replace("-", "");
		}

		/// <summary>
		///     Convert byte to nibble (4 bit value)
		/// </summary>
		/// <param name="data"> the byte to be converted to a nibble </param>
		/// <returns> A nibble of the byte supplied </returns>
		public static byte ToHexNibble(this byte data)
		{
			if (data >= 48 && data <= 57)
				return (byte) (data & 15);
			if (data >= 65 && data <= 70)
				return (byte) (data - 55);
			if (data >= 97 && data <= 102)
				return (byte) (data - 87);

			return 0;
		}

		/// <summary>
		///     Convert byte array to a 32 bit int
		/// </summary>
		/// <param name="data"> The byte array to be converted </param>
		/// <returns> An int representation of the byte array </returns>
		public static int ToInt32(this byte[] data)
		{
			return BitConverter.ToInt32(data, 0);
		}

		/// <summary>
		///     Convert a byte array into a string with each byte represented as nibbles
		/// </summary>
		/// <param name="data"> The byte array to be converted to a string of nibbles </param>
		/// <returns> A string made up of nibbles for each byte </returns>
		public static string ToNibbleString(this byte[] data)
		{
			var temp = string.Empty;
			if (data != null)
			{
				for (var i = 0; i < data.Length; i++)
				{
					temp += data[i].ToHexNibble();
				}
			}
			return temp;
		}

		private static Stream GetStream(MemoryStream Stream, CompressionMode Mode, CompressionType CompressionType)
		{
			if (CompressionType == CompressionType.Deflate)
				return new DeflateStream(Stream, Mode, true);
			return new GZipStream(Stream, Mode, true);
		}
	}
}
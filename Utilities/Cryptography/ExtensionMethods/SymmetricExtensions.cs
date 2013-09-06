using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Cryptography.ExtensionMethods
{
	/// <summary>
	///     Symmetric key extensions
	/// </summary>
	/// <remarks>
	///     Symmetric encryption and decryption is done using RijndaelManaged provider.
	///     The provider is configured to use CBC mode and PKCS7 padding.
	///     A random IV is generated each time and attached to the cipher data.
	///     The key used in the symmetric encryption is a hash of key provided using a random salt generated using
	///     Rfc2898DeriveBytes.
	///     The random salt is stored in the cipher data, the hash is generated using a high number of iterations (>1000)
	/// </remarks>
	public static class SymmetricExtensions
	{
		private const ushort _iteration = 1000;
		private const int _saltSize = 15;

		/// <summary>
		///     Decrypt cipher data using key provided
		/// </summary>
		/// <param name="cipherText">Cipher text to decrypt</param>
		/// <param name="key">Key to use to decrypt data</param>
		/// <remarks>
		///     Symmetric encryption and decryption is done using RijndaelManaged provider.
		///     The provider is configured to use CBC mode and PKCS7 padding.
		///     A random IV is generated each time and attached to the cipher data.
		///     The key used in the symmetric encryption is a hash of key provided using a random salt generated using
		///     Rfc2898DeriveBytes.
		///     The random salt is stored in the cipher data, the hash is generated using a high number of iterations (>1000)
		/// </remarks>
		/// <returns>Decrypted cipher text</returns>
		public static string Decrypt(this string cipherText, string key)
		{
			if (string.IsNullOrEmpty(cipherText))
				throw new ArgumentException("plainText");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("key");

			var cipherData = Convert.FromBase64String(cipherText);

			var plainTextData = cipherData.Decrypt(key);

			return Encoding.UTF8.GetString(plainTextData);
		}

		/// <summary>
		///     Decrypt cipher data using key provided
		/// </summary>
		/// <param name="cipherData">Cipher data to decrypt</param>
		/// <param name="key">Key to use to decrypt data</param>
		/// <remarks>
		///     Symmetric encryption and decryption is done using RijndaelManaged provider.
		///     The provider is configured to use CBC mode and PKCS7 padding.
		///     A random IV is generated each time and attached to the cipher data.
		///     The key used in the symmetric encryption is a hash of key provided using a random salt generated using
		///     Rfc2898DeriveBytes.
		///     The random salt is stored in the cipher data, the hash is generated using a high number of iterations (>1000)
		/// </remarks>
		/// <returns>Decrypted byte[]</returns>
		public static byte[] Decrypt(this byte[] cipherData, string key)
		{
			if (cipherData == null)
				throw new ArgumentException("plainTextData");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("key");

			var decryptedData = new byte[cipherData.Length];

			using (var provider = new RijndaelManaged())
			{
				provider.Mode = CipherMode.CBC;
				provider.Padding = PaddingMode.PKCS7;

				using (var memStream = new MemoryStream(cipherData))
				{
					var iv = new byte[16];
					memStream.Read(iv, 0, 16);

					var salt = new byte[_saltSize];
					memStream.Read(salt, 0, _saltSize);

					var derivedKey = new Rfc2898DeriveBytes(key, salt);
					provider.Key = derivedKey.GetBytes(provider.KeySize >> 3);

					int byteCount;
					using (var decryptor = provider.CreateDecryptor(provider.Key, iv))
					{
						using (var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
						{
							byteCount = cryptoStream.Read(decryptedData, 0, decryptedData.Length);
						}
					}

					Array.Resize(ref decryptedData, byteCount);
				}
			}
			return decryptedData;
		}

		/// <summary>
		///     Symmetric encrypt string data using the key provided
		/// </summary>
		/// <param name="plainTextData">The plain text data to be encrypted</param>
		/// <param name="key">The key to use to encrypt the data</param>
		/// <remarks>
		///     Symmetric encryption and decryption is done using RijndaelManaged provider.
		///     The provider is configured to use CBC mode and PKCS7 padding.
		///     A random IV is generated each time and attached to the cipher data.
		///     The key used in the symmetric encryption is a hash of key provided using a random salt generated using
		///     Rfc2898DeriveBytes.
		///     The random salt is stored in the cipher data, the hash is generated using a high number of iterations (>1000)
		/// </remarks>
		/// <returns>Encrypted byte[]</returns>
		public static byte[] Encrypt(this byte[] plainTextData, string key)
		{
			if (plainTextData == null)
				throw new ArgumentException("plainTextData");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("key");

			byte[] encryptedData;
			using (var provider = new RijndaelManaged())
			{
				provider.Mode = CipherMode.CBC;
				provider.Padding = PaddingMode.PKCS7;

				provider.GenerateIV();

				var derivedKey = new Rfc2898DeriveBytes(key, _saltSize, _iteration);
				provider.Key = derivedKey.GetBytes(provider.KeySize >> 3);


				using (var memStream = new MemoryStream(plainTextData.Length))
				{
					memStream.Write(provider.IV, 0, 16);
					memStream.Write(derivedKey.Salt, 0, _saltSize);

					using (var encryptor = provider.CreateEncryptor(provider.Key, provider.IV))
					{
						using (var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
						{
							cryptoStream.Write(plainTextData, 0, plainTextData.Length);
							cryptoStream.FlushFinalBlock();
						}
					}
					encryptedData = memStream.ToArray();
				}
			}
			return encryptedData;
		}

		/// <summary>
		///     Symmetric encrypt string data using the key provided
		/// </summary>
		/// <param name="plainText">The plain text to be encrypted</param>
		/// <param name="key">The key to use to encrypt the data</param>
		/// <remarks>
		///     Symmetric encryption and decryption is done using RijndaelManaged provider.
		///     The provider is configured to use CBC mode and PKCS7 padding.
		///     A random IV is generated each time and attached to the cipher data.
		///     The key used in the symmetric encryption is a hash of key provided using a random salt generated using
		///     Rfc2898DeriveBytes.
		///     The random salt is stored in the cipher data, the hash is generated using a high number of iterations (>1000)
		/// </remarks>
		/// <returns>Encrypted base 64 string </returns>
		public static string Encrypt(this string plainText, string key)
		{
			if (string.IsNullOrEmpty(plainText))
				throw new ArgumentException("plainText");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("key");

			var plainTextData = Encoding.UTF8.GetBytes(plainText);

			var cipherData = plainTextData.Encrypt(key);

			return Convert.ToBase64String(cipherData);
		}
	}
}
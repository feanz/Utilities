using System;
using System.Security.Cryptography;
using System.Text;
using Utilities.Cryptography.ExtensionMethods;

namespace Utilities.Cryptography
{
    /// <summary>
    ///   Utility class for doing RSA Encryption
    /// </summary>
    public static class RSAEncryption
    {
        /// <summary>
        ///   Creates a new set of keys
        /// </summary>
        /// <param name="privatePublic"> True if private key should be included, false otherwise </param>
        /// <returns> XML representation of the key information </returns>
        public static string CreateKey(bool privatePublic)
        {
            return new RSACryptoServiceProvider().ToXmlString(privatePublic);
        }

        /// <summary>
        ///   Decrypts a string using RSA
        /// </summary>
        /// <param name="input"> Input string (should be small as anything over 128 bytes can not be decrypted) </param>
        /// <param name="key"> Key to use for decryption </param>
        /// <param name="encodingUsing"> Encoding that the result should use (defaults to UTF8) </param>
        /// <returns> A decrypted string </returns>
        public static string Decrypt(string input, string key, Encoding encodingUsing = null)
        {
	        if (string.IsNullOrEmpty(input))
				throw new ArgumentNullException("input");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

			encodingUsing = encodingUsing ?? new UTF8Encoding();

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
				var decrypt = rsa.Decrypt(Convert.FromBase64String(input), true);
                rsa.Clear();
				return encodingUsing.GetString(decrypt);
            }
        }

        /// <summary>
        ///   Encrypts a string using RSA
        /// </summary>
        /// <param name="input"> Input string (should be small as anything over 128 bytes can not be decrypted) </param>
        /// <param name="key"> Key to use for encryption </param>
        /// <param name="encodingUsing"> Encoding that the input string uses (defaults to UTF8) </param>
        /// <returns> An encrypted string (64bit string) </returns>
        public static string Encrypt(string input, string key, Encoding encodingUsing = null)
        {
			if (string.IsNullOrEmpty(input))
				throw new ArgumentNullException("input");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

	        encodingUsing = encodingUsing ?? new UTF8Encoding();

            using (var rsa = new RSACryptoServiceProvider())
            {

                rsa.FromXmlString(key);
				var encryptedBytes = rsa.Encrypt(encodingUsing.GetBytes(input), true);
                rsa.Clear();
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        ///   Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="input"> Input string </param>
        /// <param name="key"> Key to encrypt/sign with </param>
        /// <param name="hash"> This will be filled with the unsigned hash </param>
        /// <param name="encodingUsing"> Encoding that the input is using (defaults to UTF8) </param>
		/// /// <param name="hashIdentifer">The id of the hash algorithm used (default SHA1)</param>
        /// <returns> A signed hash of the input (64bit string) </returns>
        public static string SignHash(string input, string key, out string hash, Encoding encodingUsing = null,  string hashIdentifer = "SHA1")
        {
			if (string.IsNullOrEmpty(input))
				throw new ArgumentNullException("input");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

	        encodingUsing = encodingUsing ?? new UTF8Encoding();

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
				var hashBytes = encodingUsing.GetBytes(input).Hash();
                var signedHash = rsa.SignHash(hashBytes, CryptoConfig.MapNameToOID(hashIdentifer));
                rsa.Clear();
	            hash = Convert.ToBase64String(hashBytes);
                return Convert.ToBase64String(signedHash);
            }
        }

	    /// <summary>
	    ///   Verifies a signed hash against the unsigned version
	    /// </summary>
	    /// <param name="hash"> The unsigned hash (should be 64bit string) </param>
	    /// <param name="signedHash"> The signed hash (should be 64bit string) </param>
	    /// <param name="key"> The key to use in decryption </param>
	    /// <param name="hashIdentifer">The id of the hash algorithm used (default SHA1)</param>
	    /// <returns> True if it is verified, false otherwise </returns>
	    public static bool VerifyHash(string hash, string signedHash, string key, string hashIdentifer = "SHA1")
        {
			if (string.IsNullOrEmpty(hash))
				throw new ArgumentNullException("input");

			if (string.IsNullOrEmpty(signedHash))
				throw new ArgumentNullException("signedHash");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key");

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
				var inputArray = Convert.FromBase64String(signedHash);
                var hashArray = Convert.FromBase64String(hash);
				var result = rsa.VerifyHash(hashArray, CryptoConfig.MapNameToOID(hashIdentifer), inputArray);
                rsa.Clear();
                return result;
            }
        }
    }
}
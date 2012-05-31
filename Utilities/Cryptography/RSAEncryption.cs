using System.Security.Cryptography;
using System.Text;
using Utilities.Cryptography.ExtensionMethods;
using Utilities.Extensions;

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
            input.ValidateNotNullOrEmpty("Input");
            key.ValidateNotNullOrEmpty("Key");
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
                byte[] EncryptedBytes = rsa.Decrypt(input.Base64ToByteArray(), true);
                rsa.Clear();
                return EncryptedBytes.ToEncodedString(encodingUsing);
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
            input.ValidateNotNullOrEmpty("Input");
            key.ValidateNotNullOrEmpty("Key");
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
                byte[] EncryptedBytes = rsa.Encrypt(input.ToByteArray(encodingUsing), true);
                rsa.Clear();
                return EncryptedBytes.ToBase64String();
            }
        }

        /// <summary>
        ///   Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="input"> Input string </param>
        /// <param name="key"> Key to encrypt/sign with </param>
        /// <param name="hash"> This will be filled with the unsigned hash </param>
        /// <param name="encodingUsing"> Encoding that the input is using (defaults to UTF8) </param>
        /// <returns> A signed hash of the input (64bit string) </returns>
        public static string SignHash(string input, string key, out string hash, Encoding encodingUsing = null)
        {
            input.ValidateNotNullOrEmpty("Input");
            key.ValidateNotNullOrEmpty("Key");
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
                byte[] HashBytes = input.ToByteArray(encodingUsing).Hash();
                byte[] SignedHash = rsa.SignHash(HashBytes, CryptoConfig.MapNameToOID("SHA1"));
                rsa.Clear();
                hash = HashBytes.ToBase64String();
                return SignedHash.ToBase64String();
            }
        }

        /// <summary>
        ///   Verifies a signed hash against the unsigned version
        /// </summary>
        /// <param name="hash"> The unsigned hash (should be 64bit string) </param>
        /// <param name="signedHash"> The signed hash (should be 64bit string) </param>
        /// <param name="key"> The key to use in decryption </param>
        /// <returns> True if it is verified, false otherwise </returns>
        public static bool VerifyHash(string hash, string signedHash, string key)
        {
            hash.ValidateNotNullOrEmpty("Hash");
            signedHash.ValidateNotNullOrEmpty("SignedHash");
            key.ValidateNotNullOrEmpty("Key");
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(key);
                byte[] InputArray = signedHash.Base64ToByteArray();
                byte[] HashArray = hash.Base64ToByteArray();
                bool Result = RSA.VerifyHash(HashArray, CryptoConfig.MapNameToOID("SHA1"), InputArray);
                RSA.Clear();
                return Result;
            }
        }
    }
}
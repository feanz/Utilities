using System;
using System.Security.Cryptography;
using System.Text;
using Utilities.Extensions;

namespace Utilities.Cryptography.ExtensionMethods
{
    /// <summary>
    ///   Hash based extensions
    /// </summary>
    public static class HashExtensions
    {
        /// <summary>
        ///   Computes the hash of a byte array
        /// </summary>
        /// <param name="data"> Byte array to hash </param>
        /// <param name="algorithm"> Hash algorithm to use (defaults to SHA1) </param>
        /// <returns> The hash of the byte array </returns>
        public static byte[] Hash(this byte[] data, HashAlgorithm algorithm = null)
        {
            if (data.IsNull())
                return null;
            algorithm = algorithm ?? new SHA1CryptoServiceProvider();
            using (HashAlgorithm Hasher = algorithm)
            {
                byte[] HashedArray = Hasher.ComputeHash(data);
                Hasher.Clear();
                return HashedArray;
            }
        }

        /// <summary>
        ///   Computes the hash of a string
        /// </summary>
        /// <param name="data"> string to hash </param>
        /// <param name="algorithm"> Algorithm to use (defaults to SHA1) </param>
        /// <param name="encodingUsing"> Encoding used by the string (defaults to UTF8) </param>
        /// <returns> The hash of the string </returns>
        public static string Hash(this string data, HashAlgorithm algorithm = null, Encoding encodingUsing = null)
        {
            if (data.IsNullOrEmpty())
                return "";
            return BitConverter.ToString(data.ToByteArray(encodingUsing).Hash(algorithm)).Replace("-", "");
        }
    }
}
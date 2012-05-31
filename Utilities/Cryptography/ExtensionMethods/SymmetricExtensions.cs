using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utilities.Extensions;

namespace Utilities.Cryptography.ExtensionMethods
{
    /// <summary>
    ///   Symmetric key extensions
    /// </summary>
    public static class SymmetricExtensions
    {
        /// <summary>
        ///   Decrypts a string
        /// </summary>
        /// <param name="Data"> Text to be decrypted (Base 64 string) </param>
        /// <param name="Key"> Password to decrypt with </param>
        /// <param name="EncodingUsing"> Encoding that the output string should use (defaults to UTF8) </param>
        /// <param name="AlgorithmUsing"> Algorithm to use for decryption (defaults to AES) </param>
        /// <param name="Salt"> Salt to decrypt with </param>
        /// <param name="HashAlgorithm"> Can be either SHA1 or MD5 </param>
        /// <param name="PasswordIterations"> Number of iterations to do </param>
        /// <param name="InitialVector"> Needs to be 16 ASCII characters long </param>
        /// <param name="KeySize"> Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES) </param>
        /// <returns> A decrypted string </returns>
        public static string Decrypt(this string Data, string Key,
                                     Encoding EncodingUsing = null,
                                     SymmetricAlgorithm AlgorithmUsing = null, string Salt = "Kosher",
                                     string HashAlgorithm = "SHA1", int PasswordIterations = 2,
                                     string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (Data.IsNullOrEmpty())
                return "";
            return Convert.FromBase64String(Data)
                .Decrypt(Key, AlgorithmUsing, Salt, HashAlgorithm, PasswordIterations, InitialVector, KeySize)
                .ToEncodedString(EncodingUsing);
        }

        /// <summary>
        ///   Decrypts a byte array
        /// </summary>
        /// <param name="Data"> Data to be decrypted </param>
        /// <param name="Key"> Password to decrypt with </param>
        /// <param name="AlgorithmUsing"> Algorithm to use for decryption </param>
        /// <param name="Salt"> Salt to decrypt with </param>
        /// <param name="HashAlgorithm"> Can be either SHA1 or MD5 </param>
        /// <param name="PasswordIterations"> Number of iterations to do </param>
        /// <param name="InitialVector"> Needs to be 16 ASCII characters long </param>
        /// <param name="KeySize"> Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES) </param>
        /// <returns> A decrypted byte array </returns>
        public static byte[] Decrypt(this byte[] Data, string Key,
                                     SymmetricAlgorithm AlgorithmUsing = null, string Salt = "Kosher",
                                     string HashAlgorithm = "SHA1", int PasswordIterations = 2,
                                     string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (Data.IsNull())
                return null;
            AlgorithmUsing = AlgorithmUsing ?? new RijndaelManaged();
            Key.ValidateNotNullOrEmpty("Key");
            Salt.ValidateNotNullOrEmpty("Salt");
            HashAlgorithm.ValidateNotNullOrEmpty("HashAlgorithm");
            InitialVector.ValidateNotNullOrEmpty("InitialVector");
            using (
                PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Key, Salt.ToByteArray(), HashAlgorithm,
                                                                              PasswordIterations))
            {
                using (SymmetricAlgorithm SymmetricKey = AlgorithmUsing)
                {
                    SymmetricKey.Mode = CipherMode.CBC;
                    byte[] PlainTextBytes = new byte[Data.Length];
                    int ByteCount = 0;
                    using (
                        ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(DerivedPassword.GetBytes(KeySize/8),
                                                                                  InitialVector.ToByteArray()))
                    {
                        using (MemoryStream MemStream = new MemoryStream(Data))
                        {
                            using (
                                CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read)
                                )
                            {
                                ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                                MemStream.Close();
                                CryptoStream.Close();
                            }
                        }
                    }
                    SymmetricKey.Clear();
                    Array.Resize(ref PlainTextBytes, ByteCount);
                    return PlainTextBytes;
                }
            }
        }

        /// <summary>
        ///   Encrypts a string
        /// </summary>
        /// <param name="Data"> Text to be encrypted </param>
        /// <param name="Key"> Password to encrypt with </param>
        /// <param name="AlgorithmUsing"> Algorithm to use for encryption (defaults to AES) </param>
        /// <param name="Salt"> Salt to encrypt with </param>
        /// <param name="HashAlgorithm"> Can be either SHA1 or MD5 </param>
        /// <param name="PasswordIterations"> Number of iterations to do </param>
        /// <param name="InitialVector"> Needs to be 16 ASCII characters long </param>
        /// <param name="KeySize"> Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES) </param>
        /// <param name="EncodingUsing"> Encoding that the original string is using (defaults to UTF8) </param>
        /// <returns> An encrypted string (Base 64 string) </returns>
        public static string Encrypt(this string Data, string Key,
                                     Encoding EncodingUsing = null,
                                     SymmetricAlgorithm AlgorithmUsing = null, string Salt = "Kosher",
                                     string HashAlgorithm = "SHA1", int PasswordIterations = 2,
                                     string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (Data.IsNullOrEmpty())
                return "";
            return Data.ToByteArray(EncodingUsing)
                .Encrypt(Key, AlgorithmUsing, Salt, HashAlgorithm, PasswordIterations, InitialVector, KeySize)
                .ToBase64String();
        }

        /// <summary>
        ///   Encrypts a byte array
        /// </summary>
        /// <param name="Data"> Data to be encrypted </param>
        /// <param name="Key"> Password to encrypt with </param>
        /// <param name="AlgorithmUsing"> Algorithm to use for encryption (defaults to AES) </param>
        /// <param name="Salt"> Salt to encrypt with </param>
        /// <param name="HashAlgorithm"> Can be either SHA1 or MD5 </param>
        /// <param name="PasswordIterations"> Number of iterations to do </param>
        /// <param name="InitialVector"> Needs to be 16 ASCII characters long </param>
        /// <param name="KeySize"> Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES) </param>
        /// <returns> An encrypted byte array </returns>
        public static byte[] Encrypt(this byte[] Data, string Key,
                                     SymmetricAlgorithm AlgorithmUsing = null, string Salt = "Kosher",
                                     string HashAlgorithm = "SHA1", int PasswordIterations = 2,
                                     string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (Data.IsNull())
                return null;
            AlgorithmUsing = AlgorithmUsing ?? new RijndaelManaged();
            Key.ValidateNotNullOrEmpty("Key");
            Salt.ValidateNotNullOrEmpty("Salt");
            HashAlgorithm.ValidateNotNullOrEmpty("HashAlgorithm");
            InitialVector.ValidateNotNullOrEmpty("InitialVector");
            using (
                PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Key, Salt.ToByteArray(), HashAlgorithm,
                                                                              PasswordIterations))
            {
                using (SymmetricAlgorithm SymmetricKey = AlgorithmUsing)
                {
                    SymmetricKey.Mode = CipherMode.CBC;
                    byte[] CipherTextBytes = null;
                    using (
                        ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(DerivedPassword.GetBytes(KeySize/8),
                                                                                  InitialVector.ToByteArray()))
                    {
                        using (MemoryStream MemStream = new MemoryStream())
                        {
                            using (
                                CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor,
                                                                             CryptoStreamMode.Write))
                            {
                                CryptoStream.Write(Data, 0, Data.Length);
                                CryptoStream.FlushFinalBlock();
                                CipherTextBytes = MemStream.ToArray();
                                MemStream.Close();
                                CryptoStream.Close();
                            }
                        }
                    }
                    SymmetricKey.Clear();
                    return CipherTextBytes;
                }
            }
        }
    }
}
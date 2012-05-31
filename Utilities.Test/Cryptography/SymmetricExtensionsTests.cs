using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.Cryptography.ExtensionMethods;

namespace Utilities.Test.Cryptography
{
    [TestClass]
    public class SymmetricExtensionsTests
    {
        [TestMethod]
        public void Symmetric_decrypt_should_reproduce_plaintext_from_encrypted_string()
        {
            var encrypted = "plainText".Encrypt("passkey", "koshersalt");
            var decrypted = encrypted.Decrypt("passkey", "koshersalt");

            Assert.AreEqual("plainText", decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void Symmetric_decrypt_should_not_reproduce_plaintext_when_key_is_incorrect()
        {
            var encrypted = "plainText".Encrypt("passkey", "koshersalt");
            var decrypted = encrypted.Decrypt("wrongkey", "koshersalt");

            Assert.AreNotEqual("plainText", decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void Symmetric_decrypt_should_not_reproduce_plaintext_when_salt_is_incorrect()
        {
            var encrypted = "plainText".Encrypt("passkey", "koshersalt");
            var decrypted = encrypted.Decrypt("passkey", "wrongsalt");

            Assert.AreNotEqual("plainText", decrypted);
        }

        [TestMethod]
        public void encrypt_decrypt_should_work_using_variaes_algorithm_and_keys_sizes()
        {
            const string data = "This is a test of the system.";
            Assert.AreNotEqual("This is a test of the system.", data.Encrypt("Babysfirstkey", "koshersalt"));
            Assert.AreEqual("This is a test of the system.", data.Encrypt("Babysfirstkey", "koshersalt").Decrypt("Babysfirstkey", "koshersalt"));
            Assert.AreEqual("This is a test of the system.", data.Encrypt("Babysfirstkey", "koshersalt", algorithmUsing: new DESCryptoServiceProvider(), keySize: 64).Decrypt("Babysfirstkey","koshersalt", algorithmUsing: new DESCryptoServiceProvider(), keySize: 64));
            Assert.AreEqual("This is a test of the system.", data.Encrypt("Babysfirstkey", "koshersalt", algorithmUsing: new TripleDESCryptoServiceProvider(), keySize: 192).Decrypt("Babysfirstkey","koshersalt", algorithmUsing: new TripleDESCryptoServiceProvider(), keySize: 192));
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.Cryptography.ExtensionMethods;

namespace Utilities.Test.Cryptography
{
    [TestClass]
    public class SymmetricExtensionsTests
    {
	    [TestMethod]
	    public void Can_encrypt_and_decrypt_random_string_data()
	    {
		    for (int i = 0; i < 1000; i++)
		    {
				var plaintext = DataHelper.RandomString(100);

				var cipherText = plaintext.Encrypt("KEY");

				var decrypt = cipherText.Decrypt("KEY");

				Assert.AreEqual(plaintext, decrypt);    
		    }
	    }
    }
}
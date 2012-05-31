using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.Cryptography;

namespace Utilities.Test.Cryptography
{
    [TestClass]
    public class BCryptTests
    {
        [TestMethod]
        public void two_hashes_using_same_password_and_salt_should_match()
        {
            const string password = "Password";
            var salt = BCrypt.GenerateSalt(12);
            var hash1 = BCrypt.HashPassword(password, salt);
            var hash2 = BCrypt.HashPassword(password, salt);
            
            Assert.AreEqual(hash1, hash2);
        }

        [TestMethod]
        public void two_hashes_using_same_password_and_diff_salt_should_not_match()
        {
            const string password = "Password";
            var hash1 = BCrypt.HashPassword(password, BCrypt.GenerateSalt(12));
            var hash2 = BCrypt.HashPassword(password, BCrypt.GenerateSalt(12));

            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void salt_generation_time_should_double_for_every_extra_charater_in_salt()
        {
            var watch = new Stopwatch();
            watch.Start();
            var salt1 = BCrypt.GenerateSalt(12);
            watch.Stop();

            var elapsed1 = watch.ElapsedMilliseconds;
            watch.Start();
            var salt2 = BCrypt.GenerateSalt(13);
            watch.Stop();

            var elapsed2 = watch.ElapsedMilliseconds;

            if (elapsed2 > elapsed1 * 2)
                Assert.Fail("Generation of salt was too fast");
        }
    }
}
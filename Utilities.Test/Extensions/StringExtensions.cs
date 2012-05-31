using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.Extensions;

namespace Utilities.Test.Extensions
{
    [TestClass]
    public class StringExtensions
    {
        [TestMethod]
        public void is_binary_should_only_return_truw_for_strings_which_are_valid_binary()
        {
            Assert.IsTrue("000001".IsBinary());
            Assert.IsTrue("010101".IsBinary());
            Assert.IsFalse("010102".IsBinary());
            Assert.IsFalse("SPAM".IsBinary());
        }

        [TestMethod]
        public void is_hex_should_only_return_truw_for_strings_which_are_valid_hex()
        {
            Assert.IsTrue("000000".IsHex());
            Assert.IsTrue("010101".IsHex());
            Assert.IsFalse("J10102".IsHex());
            Assert.IsFalse("SPAM".IsHex());
        }

        [TestMethod]
        public void binary_to_byte_array_should_convert_a_valid_string_representation_of_binary()
        {
            "00000000".BinaryToByteArray();
            "00000001".BinaryToByteArray();
            "01010101".BinaryToByteArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void binary_to_byte_array_should_throw_argument_exception_when_string_invalid_lenth()
        {
            //To convert to byte array binary must be a correct byte multiple e.g. / 8
            "0000000".BinaryToByteArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void binary_to_byte_array_should_throw_argument_exception_when_string_invalid_format()
        {
            "Steve".BinaryToByteArray();
        }
    }
}
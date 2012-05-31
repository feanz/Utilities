using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.Extensions;

namespace Utilities.Test.Extensions
{
    [TestClass]
    public class ByteExtensionTests
    {
        [TestMethod]
        public void append_should_combine_two_byte_arrays()
        {
            var firstData = new byte[] {0x01, 0x02};
            var seconddata = new byte [] { 0x03, 0x04 };

            var appended = firstData.Append(seconddata);

            Assert.AreEqual(appended.Length, firstData.Length + seconddata.Length);
            Assert.AreEqual(appended[0], firstData[0]);
            Assert.AreEqual(appended[1], firstData[1]);
            Assert.AreEqual(appended[2], seconddata[0]);
            Assert.AreEqual(appended[3], seconddata[1]);
        }

        [TestMethod]
        public void convert_string_to_byte_array_and_then_convert_back()
        {
            const string data = "This is a bit of data";
            Assert.AreEqual("This is a bit of data",data.ToByteArray().ToEncodedString());
            Assert.AreNotEqual("This is a bit the wrong data", data.ToByteArray().ToEncodedString());

            Assert.AreEqual("This is a bit of data", data.ToByteArray(Encoding.ASCII).ToEncodedString(Encoding.ASCII));
            Assert.AreNotEqual("This is a bit the wrong data", data.ToByteArray(Encoding.ASCII).ToEncodedString(Encoding.ASCII));
        }

        [TestMethod]
        public void DeflateTest()
        {
            const string data = "This is a bit of data that I want to compress";
            Assert.AreNotEqual("This is a bit of data that I want to compress", data.ToByteArray().Compress(ByteExtensions.CompressionType.Deflate).ToEncodedString());
            Assert.AreEqual("This is a bit of data that I want to compress", data.ToByteArray().Compress(ByteExtensions.CompressionType.Deflate).Decompress(ByteExtensions.CompressionType.Deflate).ToEncodedString());
            Assert.AreEqual("This is a bit of data that I want to compress", data.Compress(compressionType: ByteExtensions.CompressionType.Deflate).Decompress(compressionType: ByteExtensions.CompressionType.Deflate));
        }

        [TestMethod]
        public void GZipTest()
        {
            const string data = "This is a bit of data that I want to compress";
            Assert.AreNotEqual("This is a bit of data that I want to compress", data.ToByteArray().Compress().ToEncodedString());
            Assert.AreEqual("This is a bit of data that I want to compress", data.ToByteArray().Compress().Decompress().ToEncodedString());
            Assert.AreEqual("This is a bit of data that I want to compress", data.Compress().Decompress());
        }
    }
}
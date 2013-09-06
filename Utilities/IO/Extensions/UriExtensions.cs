using System;
using System.IO;
using System.Net;

namespace Utilities.IO.Extensions
{
    /// <summary>
    ///   Uri Extension methods
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
		///   Reads the text content of a URI
        /// </summary>
        /// <param name="uri"> Uri to read the content of </param>
        /// <param name="userName"> User name used in network credentials </param>
        /// <param name="password"> Password used in network credentials </param>
		/// <returns> String representation of the content of the URI </returns>
        public static string Read(this Uri uri, string userName = "", string password = "")
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            using (var client = new WebClient())
            {
                using (var reader = new StreamReader(uri.Read(client, userName, password)))
                {
                    var contents = reader.ReadToEnd();
                    reader.Close();
	                return contents;
                }
            }
        }

        /// <summary>
		///   Reads the text content of a URI
        /// </summary>
        /// <param name="uri"> The Uri to read the content of </param>
        /// <param name="client"> WebClient used to load the data </param>
        /// <param name="userName"> User name used in network credentials </param>
        /// <param name="password"> Password used in network credentials </param>
		/// <returns> Stream containing the content of the URI </returns>
        public static Stream Read(this Uri uri, WebClient client, string userName = "", string password = "")
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                client.Credentials = new NetworkCredential(userName, password);
            return client.OpenRead(uri);
        }

        /// <summary>
		///   Reads the content of a URI
        /// </summary>
        /// <param name="uri"> Uri to read the content of </param>
        /// <param name="userName"> User name used in network credentials </param>
        /// <param name="password"> Password used in network credentials </param>
		/// <returns> Byte array representation of the content of the URI </returns>
        public static byte[] ReadBinary(this Uri uri, string userName = "", string password = "")
        {
            if (uri == null)
				throw new ArgumentNullException("uri");
            using (var client = new WebClient())
            {
                using (var reader = uri.Read(client, userName, password))
                {
                    using (var finalStream = new MemoryStream())
                    {
                        while (true)
                        {
                            var buffer = new byte[1024];
                            var count = reader.Read(buffer, 0, buffer.Length);
                            finalStream.Write(buffer, 0, count);
                            if (count < buffer.Length)
                                break;
                        }
                        var returnValue = finalStream.ToArray();
                        reader.Close();
                        finalStream.Close();
                        return returnValue;
                    }
                }
            }
        }
    }
}
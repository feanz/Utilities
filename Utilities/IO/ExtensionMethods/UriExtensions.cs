using System;
using System.IO;
using System.Net;
using Utilities.Extensions;

namespace Utilities.IO.ExtensionMethods
{
    /// <summary>
    ///   Uri Extension methods
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        ///   Reads the text content of a url
        /// </summary>
        /// <param name="url"> Uri to read the content of </param>
        /// <param name="userName"> User name used in network credentials </param>
        /// <param name="password"> Password used in network credentials </param>
        /// <returns> String representation of the content of the url </returns>
        public static string Read(this Uri url, string userName = "", string password = "")
        {
            if (url.IsNull())
                throw new ArgumentNullException("url");
            using (var client = new WebClient())
            {
                using (var reader = new StreamReader(url.Read(client, userName, password)))
                {
                    var contents = reader.ReadToEnd();
                    reader.Close();
                    return contents;
                }
            }
        }

        /// <summary>
        ///   Reads the text content of a url
        /// </summary>
        /// <param name="url"> The Uri to read the content of </param>
        /// <param name="client"> WebClient used to load the data </param>
        /// <param name="userName"> User name used in network credentials </param>
        /// <param name="password"> Password used in network credentials </param>
        /// <returns> Stream containing the content of the url </returns>
        public static Stream Read(this Uri url, WebClient client, string userName = "", string password = "")
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                client.Credentials = new NetworkCredential(userName, password);
            return client.OpenRead(url);
        }

        /// <summary>
        ///   Reads the content of a url
        /// </summary>
        /// <param name="url"> Uri to read the content of </param>
        /// <param name="userName"> User name used in network credentials </param>
        /// <param name="password"> Password used in network credentials </param>
        /// <returns> Byte array representation of the content of the url </returns>
        public static byte[] ReadBinary(this Uri url, string userName = "", string password = "")
        {
            if (url.IsNull())
                throw new ArgumentNullException("url");
            using (var client = new WebClient())
            {
                using (var reader = url.Read(client, userName, password))
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
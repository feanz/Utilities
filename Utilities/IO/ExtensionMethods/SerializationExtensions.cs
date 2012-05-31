using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security;
using System.Text;
using System.Xml;
using ServiceStack.Text;
using Utilities.Extensions;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace Utilities.IO.ExtensionMethods
{
    /// <summary>
    ///   Serialization extensions
    /// </summary>
    public static class SerializationExtensions
    {
        /// <summary>
        ///   Converts a JSON file to an object of the specified type
        /// </summary>
        /// <typeparam name="T"> Object type to return </typeparam>
        /// <param name="jsonFile"> The file to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The file converted to the specified object type </returns>
        public static T JsonToObject<T>(this FileInfo jsonFile, Encoding encodingUsing = null)
        {
            return (jsonFile.IsNull() || !jsonFile.Exists)
                       ? default(T)
                       : (T) jsonFile.Read().JsonToObject<T>(encodingUsing);
        }

        /// <summary>
        ///   Converts a JSON string to an object of the specified type
        /// </summary>
        /// <param name="json"> The string to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The string converted to the specified object type </returns>
        public static T JsonToObject<T>(this string json, Encoding encodingUsing = null)
        {
            json.ValidateNotNullOrEmpty("json");

            if (encodingUsing.IsNull())
                encodingUsing = new ASCIIEncoding();
            using (var stream = new MemoryStream(encodingUsing.GetBytes(json)))
            {
                return JsonSerializer.DeserializeFromStream<T>(stream);
            }
        }

        /// <summary>
        ///   Converts a SOAP string to an object of the specified type
        /// </summary>
        /// <typeparam name="T"> Object type to return </typeparam>
        /// <param name="soap"> The string to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The string converted to the specified object type </returns>
        public static T SoapToObject<T>(this string soap, Encoding encodingUsing = null)
        {
            return soap.IsNullOrEmpty() ? default(T) : (T) soap.SoapToObject(typeof (T), encodingUsing);
        }

        /// <summary>
        ///   Converts a SOAP file to an object of the specified type
        /// </summary>
        /// <typeparam name="T"> Object type to return </typeparam>
        /// <param name="soapFile"> The file to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The file converted to the specified object type </returns>
        public static T SoapToObject<T>(this FileInfo soapFile, Encoding encodingUsing = null)
        {
            return (soapFile.IsNull() || !soapFile.Exists)
                       ? default(T)
                       : (T) soapFile.Read().SoapToObject(typeof (T), encodingUsing);
        }

        /// <summary>
        ///   Converts a SOAP string to an object of the specified type
        /// </summary>
        /// <param name="objectType"> Object type to return </param>
        /// <param name="soap"> The string to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The string converted to the specified object type </returns>
        [SecuritySafeCritical]
        public static object SoapToObject(this string soap, Type objectType, Encoding encodingUsing = null)
        {
            if (string.IsNullOrEmpty(soap))
                return null;
            if (encodingUsing.IsNull())
                encodingUsing = new ASCIIEncoding();
            using (var stream = new MemoryStream(encodingUsing.GetBytes(soap)))
            {
                var formatter = new SoapFormatter();
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        ///   Converts a SOAP file to an object of the specified type
        /// </summary>
        /// <param name="objectType"> Object type to return </param>
        /// <param name="soapFile"> The file to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The file converted to the specified object type </returns>
        public static object SoapToObject(this FileInfo soapFile, Type objectType, Encoding encodingUsing = null)
        {
            return (soapFile.IsNull() || !soapFile.Exists)
                       ? null
                       : soapFile.Read().SoapToObject(objectType, encodingUsing);
        }

        /// <summary>
        ///   Converts an object to Binary
        /// </summary>
        /// <param name="instance"> Object to convert </param>
        /// <param name="fileToSaveTo"> File to save the XML to (optional) </param>
        /// <returns> The object converted to a JSON string </returns>
        public static byte[] ToBinary(this object instance, string fileToSaveTo = "")
        {
            instance.ValidateNotNull("Object");

            byte[] output;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, instance);
                output = stream.ToArray();
            }
            if (!string.IsNullOrEmpty(fileToSaveTo))
                new FileInfo(fileToSaveTo).Save(output);
            return output;
        }

        /// <summary>
        ///   Converts an object to JSON
        /// </summary>
        /// <param name="instance"> Object to convert </param>
        /// <param name="fileToSaveTo"> File to save the XML to (optional) </param>
        /// <param name="encodingUsing"> Encoding that the XML should be saved/returned as (defaults to ASCII) </param>
        /// <returns> The object converted to a JSON string </returns>
        public static string ToJSON(this object instance, string fileToSaveTo = "", Encoding encodingUsing = null)
        {
            instance.ValidateNotNull("Object");

            encodingUsing = encodingUsing ?? new ASCIIEncoding();

            string JSON;
            using (var stream = new MemoryStream())
            {
                JsonSerializer.SerializeToString(stream);
                stream.Flush();
                JSON = encodingUsing.GetString(stream.GetBuffer(), 0, (int) stream.Position);
            }
            if (!string.IsNullOrEmpty(fileToSaveTo))
                new FileInfo(fileToSaveTo).Save(JSON);
            return JSON;
        }

        /// <summary>
        ///   Converts the serialized byte array into an object
        /// </summary>
        /// <param name="data"> The byte array to convert </param>
        /// <typeparam name="T"> Object type to return </typeparam>
        /// <returns> The byte array converted to the specified object type </returns>
        public static T ToObject<T>(this byte[] data)
        {
            return (data.IsNull()) ? default(T) : (T) data.ToObject(typeof (T));
        }

        /// <summary>
        ///   Converts the serialized byte array into an object
        /// </summary>
        /// <param name="data"> The byte array to convert </param>
        /// <param name="objectType"> Object type to return </param>
        /// <returns> The byte array converted to the specified object type </returns>
        public static object ToObject(this byte[] data, Type objectType)
        {
            if (data.IsNull())
                return null;
            using (var Stream = new MemoryStream(data))
            {
                var Formatter = new BinaryFormatter();
                return Formatter.Deserialize(Stream);
            }
        }

        /// <summary>
        ///   Converts the serialized XML document into an object
        /// </summary>
        /// <param name="document"> The XML document to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <typeparam name="T"> Object type to return </typeparam>
        /// <returns> The XML document converted to the specified object type </returns>
        public static T ToObject<T>(this XmlDocument document, Encoding encodingUsing = null)
        {
            return (document.IsNull()) ? default(T) : (T) document.InnerXml.XmlToObject(typeof (T), encodingUsing);
        }

        /// <summary>
        ///   Converts the serialized XML document into an object
        /// </summary>
        /// <param name="content"> The XML document to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <param name="objectType"> Object type to return </param>
        /// <returns> The XML document converted to the specified object type </returns>
        public static object ToObject(this XmlDocument content, Type objectType, Encoding encodingUsing = null)
        {
            return (content.IsNull()) ? null : content.InnerXml.XmlToObject(objectType, encodingUsing);
        }

        /// <summary>
        ///   Converts an object to a SOAP string
        /// </summary>
        /// <param name="instance"> Object to serialize </param>
        /// <param name="fileToSaveTo"> File to save the XML to (optional) </param>
        /// <param name="encodingUsing"> Encoding that the XML should be saved/returned as (defaults to ASCII) </param>
        /// <returns> The serialized string </returns>
        [SecuritySafeCritical]
        public static string ToSOAP(this object instance, string fileToSaveTo = "", Encoding encodingUsing = null)
        {
            instance.ValidateNotNull("Object");

            encodingUsing = encodingUsing ?? new ASCIIEncoding();

            string SOAP;
            using (var stream = new MemoryStream())
            {
                var serializer = new SoapFormatter();
                serializer.Serialize(stream, instance);
                stream.Flush();
                SOAP = encodingUsing.GetString(stream.GetBuffer(), 0, (int) stream.Position);
            }
            if (!string.IsNullOrEmpty(fileToSaveTo))
                new FileInfo(fileToSaveTo).Save(SOAP);
            return SOAP;
        }

        /// <summary>
        ///   Converts an object to XML
        /// </summary>
        /// <param name="instance"> Object to convert </param>
        /// <param name="fileToSaveTo"> File to save the XML to (optional) </param>
        /// <param name="encodingUsing"> Encoding that the XML should be saved/returned as (defaults to ASCII) </param>
        /// <returns> string representation of the object in XML format </returns>
        public static string ToXML(this object instance, string fileToSaveTo = "", Encoding encodingUsing = null)
        {
            instance.ValidateNotNull("Object");

            encodingUsing = encodingUsing ?? new ASCIIEncoding();

            string XML;
            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(instance.GetType());
                serializer.Serialize(stream, instance);
                stream.Flush();
                XML = encodingUsing.GetString(stream.GetBuffer(), 0, (int) stream.Position);
            }
            if (!string.IsNullOrEmpty(fileToSaveTo))
                new FileInfo(fileToSaveTo).Save(XML);
            return XML;
        }

        /// <summary>
        ///   Converts a string to an object of the specified type
        /// </summary>
        /// <typeparam name="T"> Object type to return </typeparam>
        /// <param name="xml"> The string to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The string converted to the specified object type </returns>
        public static T XmlToObject<T>(this string xml, Encoding encodingUsing = null)
        {
            return xml.IsNullOrEmpty() ? default(T) : (T) xml.XmlToObject(typeof (T), encodingUsing);
        }

        /// <summary>
        ///   Converts a FileInfo object to an object of the specified type
        /// </summary>
        /// <typeparam name="T"> Object type to return </typeparam>
        /// <param name="fileInfo"> The file to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The file converted to the specified object type </returns>
        public static T XmlToObject<T>(this FileInfo fileInfo, Encoding encodingUsing = null)
        {
            return (fileInfo.IsNull() || !fileInfo.Exists)
                       ? default(T)
                       : (T) fileInfo.Read().XmlToObject(typeof (T), encodingUsing);
        }

        /// <summary>
        ///   Converts a string to an object of the specified type
        /// </summary>
        /// <param name="objectType"> Object type to return </param>
        /// <param name="xml"> The string to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The string converted to the specified object type </returns>
        public static object XmlToObject(this string xml, Type objectType, Encoding encodingUsing = null)
        {
            if (xml.IsNullOrEmpty())
                return null;
            if (encodingUsing.IsNull())
                encodingUsing = new ASCIIEncoding();
            using (var stream = new MemoryStream(encodingUsing.GetBytes(xml)))
            {
                var serializer = new XmlSerializer(objectType);
                return serializer.Deserialize(stream);
            }
        }

        /// <summary>
        ///   Converts a FileInfo object to an object of the specified type
        /// </summary>
        /// <param name="objectType"> Object type to return </param>
        /// <param name="fileInfo"> The file to convert </param>
        /// <param name="encodingUsing"> Encoding to use (defaults to ASCII) </param>
        /// <returns> The file converted to the specified object type </returns>
        public static object XmlToObject(this FileInfo fileInfo, Type objectType, Encoding encodingUsing = null)
        {
            return (fileInfo.IsNull() || !fileInfo.Exists)
                       ? null
                       : fileInfo.Read().XmlToObject(objectType, encodingUsing);
        }
    }
}
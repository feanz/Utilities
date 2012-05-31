using System;
using System.IO;
using System.Text;
using Utilities.Extensions;

namespace Utilities.IO.ExtensionMethods
{
    /// <summary>
    ///   Extension methods for <see cref="System.IO.FileInfo" />
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        ///   Appends a string to a file
        /// </summary>
        /// <param name="file"> File to append to </param>
        /// <param name="content"> Content to save to the file </param>
        /// <param name="encodingUsing"> The type of encoding the string is using (defaults to ASCII) </param>
        /// <returns> The FileInfo object </returns>
        public static FileInfo Append(this FileInfo file, string content, Encoding encodingUsing = null)
        {
            file.ValidateNotNull("File");

            encodingUsing = encodingUsing ?? new ASCIIEncoding();
            byte[] ContentBytes = encodingUsing.GetBytes(content);
            return file.Append(ContentBytes);
        }

        /// <summary>
        ///   Appends a byte array to a file
        /// </summary>
        /// <param name="file"> File to append to </param>
        /// <param name="content"> Content to append to the file </param>
        /// <returns> The FileInfo object </returns>
        public static FileInfo Append(this FileInfo file, byte[] content)
        {
            file.ValidateNotNull("File");

            if (!file.Exists)
                return file.Save(content);
            using (FileStream writer = file.Open(FileMode.Append, FileAccess.Write))
            {
                writer.Write(content, 0, content.Length);
                writer.Close();
            }
            return file;
        }

        /// <summary>
        ///   Compares two files against one another
        /// </summary>
        /// <param name="file1"> First file </param>
        /// <param name="file2"> Second file </param>
        /// <returns> True if the content is the same, false otherwise </returns>
        public static bool CompareTo(this FileInfo file1, FileInfo file2)
        {
            if (file1.IsNull() || !file1.Exists)
                throw new ArgumentNullException("file1");
            if (file2.IsNull() || !file2.Exists)
                throw new ArgumentNullException("file2");
            if (file1.Length != file2.Length)
                return false;
            if (!file1.Read().Equals(file2.Read()))
                return false;
            return true;
        }

        /// <summary>
        ///   Reads a file to the end as a string
        /// </summary>
        /// <param name="file"> File to read </param>
        /// <returns> A string containing the contents of the file </returns>
        public static string Read(this FileInfo file)
        {
            file.ValidateNotNull("File");

            if (!file.Exists)
                return "";
            using (StreamReader reader = file.OpenText())
            {
                string Contents = reader.ReadToEnd();
                reader.Close();
                return Contents;
            }
        }

        /// <summary>
        ///   Reads a file to the end and returns a binary array
        /// </summary>
        /// <param name="file"> File to open </param>
        /// <returns> A binary array containing the contents of the file </returns>
        public static byte[] ReadBinary(this FileInfo file)
        {
            file.ValidateNotNull("File");

            if (!file.Exists)
                return new byte[0];
            using (var Reader = file.OpenRead())
            {
                using (var tempReader = new MemoryStream())
                {
                    while (true)
                    {
                        var Buffer = new byte[1024];
                        var count = Reader.Read(Buffer, 0, 1024);
                        tempReader.Write(Buffer, 0, count);
                        if (count < 1024)
                            break;
                    }
                    Reader.Close();
                    byte[] Output = tempReader.ToArray();
                    tempReader.Close();
                    return Output;
                }
            }
        }

        /// <summary>
        ///   Saves a string to a file
        /// </summary>
        /// <param name="file"> File to save to </param>
        /// <param name="content"> Content to save to the file </param>
        /// <param name="encodingUsing"> Encoding that the content is using (defaults to ASCII) </param>
        /// <returns> The FileInfo object </returns>
        public static FileInfo Save(this FileInfo file, string content, Encoding encodingUsing = null)
        {
            file.ValidateNotNull("File");

            if (encodingUsing.IsNull())
                encodingUsing = new ASCIIEncoding();
            byte[] ContentBytes = encodingUsing.GetBytes(content);
            return file.Save(ContentBytes);
        }

        /// <summary>
        ///   Saves a byte array to a file
        /// </summary>
        /// <param name="file"> File to save to </param>
        /// <param name="content"> Content to save to the file </param>
        /// <returns> The FileInfo object </returns>
        public static FileInfo Save(this FileInfo file, byte[] content)
        {
            file.ValidateNotNull("File");

            if (file.DirectoryName.IsNotNull()) new DirectoryInfo(file.DirectoryName).Create();
            using (var Writer = file.Create())
            {
                Writer.Write(content, 0, content.Length);
                Writer.Close();
            }
            return file;
        }

        /// <summary>
        ///   Saves a string to a file (asynchronously)
        /// </summary>
        /// <param name="file"> File to save to </param>
        /// <param name="content"> Content to save to the file </param>
        /// <param name="callBack"> Call back function </param>
        /// <param name="stateObject"> State object </param>
        /// <param name="encodingUsing"> Encoding that the content is using (defaults to ASCII) </param>
        /// <returns> The FileInfo object </returns>
        public static FileInfo SaveAsync(this FileInfo file, string content, AsyncCallback callBack, object stateObject,
                                         Encoding encodingUsing = null)
        {
            file.ValidateNotNull("File");

            if (encodingUsing.IsNull())
                encodingUsing = new ASCIIEncoding();
            if (encodingUsing.IsNotNull())
            {
                var ContentBytes = encodingUsing.GetBytes(content);
                return file.SaveAsync(ContentBytes, callBack, stateObject);
            }
            return null;
        }

        /// <summary>
        ///   Saves a byte array to a file (asynchronously)
        /// </summary>
        /// <param name="file"> File to save to </param>
        /// <param name="content"> Content to save to the file </param>
        /// <param name="callBack"> Call back function </param>
        /// <param name="stateObject"> State object </param>
        /// <returns> The FileInfo object </returns>
        public static FileInfo SaveAsync(this FileInfo file, byte[] content, AsyncCallback callBack, object stateObject)
        {
            file.ValidateNotNull("File");

            if (file.IsNotNull())
            {
                new DirectoryInfo(file.DirectoryName).Create();
                using (var Writer = file.Create())
                {
                    Writer.BeginWrite(content, 0, content.Length, callBack, stateObject);
                    Writer.Close();
                }
                return file;
            }
            return null;
        }

        /// <summary>
        ///   Sets the attributes of a file
        /// </summary>
        /// <param name="file"> File </param>
        /// <param name="attributes"> Attributes to set </param>
        /// <returns> The file info </returns>
        public static FileInfo SetAttributes(this FileInfo file, FileAttributes attributes)
        {
            if (file.IsNull() || !file.Exists)
                throw new ArgumentNullException("file");
            File.SetAttributes(file.FullName, attributes);
            return file;
        }

        /// <summary>
        /// Get the file name of the current file without extension
        /// </summary>
        /// <param name="file">Current file info</param>
        /// <returns>The file name without extension</returns>
        public static string FileNameNoExtension(this FileInfo file)
        {
            return Path.GetFileNameWithoutExtension(file.FullName);
        }
    }
}